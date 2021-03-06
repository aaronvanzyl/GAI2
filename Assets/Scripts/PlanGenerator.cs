using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlanGenerator
{
    static readonly float maxTimePerAction = 10000;

    public static PlanSet GeneratePlanSet(IReadOnlyWorld underlying, int actorID, Condition condition, int maxBranch, int maxDepth) {
        ExpandTree(underlying, actorID, condition, maxBranch, maxDepth);
        List<PathNode> planRootNodes = GeneratePathTrees(condition);
        PlanSet planSet = new PlanSet(underlying, condition, actorID);
        foreach (PathNode planRoot in planRootNodes) {
            bool valid = false;
            IReadOnlyDictionary<int, int> varDict = null;
            IReadOnlyDictionary<int, float> costs = null;
            float totalCost = float.NaN;
            IReadOnlyDictionary<int, IEnumerable<Inequality>> inequalities = GenerateInequalities(underlying, planRoot);
            List<Inequality> allInequalities = new List<Inequality>();
            foreach (IEnumerable<Inequality> ls in inequalities.Values)
            {
                allInequalities.AddRange(ls);
            }
           
            if (IneqSolver.Solve(allInequalities, out Dictionary<int, int> solvedVars))
            {
                valid = true;
                varDict = solvedVars;
                costs = CalculateCosts(underlying, planRoot, varDict);
                totalCost = costs.Values.Sum();
            }
            Plan plan = new Plan(planRoot, valid, DFS(planRoot), varDict, inequalities, costs, totalCost);
            planSet.plans.Add(plan);
            if (plan.valid) {
                planSet.validPlans.Add(plan);
            }
        }
        return planSet;
    }

    /// <summary>
    /// Expands condition/action tree in-place
    /// </summary>
    /// <param name="condition">Root condition</param>
    /// <param name="maxBranch">Max number of actions per condition</param>
    /// <param name="maxDepth">Max depth of tree (in actions)</param>
    /// 
    static void ExpandTree(IReadOnlyWorld world, int actorID, Condition condition, int maxBranch, int maxDepth)
    {
        if (maxDepth == 0)
        {
            return;
        }
        condition.actions = condition.GenerateActions(world, actorID, maxBranch);
        foreach (Action action in condition.actions)
        {
            action.conditions = action.GenerateConditions(world);
            foreach (Condition cond in action.conditions)
            {
                ExpandTree(world, actorID, cond, maxBranch, maxDepth - 1);
            }
        }
        int nextID = 0;
        foreach (Node n in DFS(condition)) {
            n.ID = nextID;
            nextID++;
        }
    }

    // Returns a new list of paths, where every path has a final world state ending with the (probably satisfied) given condition
    static List<PathNode> GeneratePathTrees(Condition condition)
    {
        List<PathNode> paths = Paths(condition);
        int nextID = 0;
        foreach (PathNode pathRoot in paths) {
            foreach (PathNode n in DFS(pathRoot)) {
                n.ID = nextID;
                nextID++;
            }
        }
        return paths;
    }

    // Returns a new list of paths, where every path has a final world state ending with the (probably satisfied) given condition
    static List<PathNode> Paths(Condition condition)
    {
        if (condition.actions.Count == 0)
        {
            return new List<PathNode>() { new PathNode(condition) };
        }
        List<PathNode> condPaths = new List<PathNode>();
        //Debug.Log($"Evaluating {condition.actions.Count} actions to solve {condition.GetName()}");
        foreach (Action action in condition.actions)
        {
            //Debug.Log($"Evaluating {action.GetName()} to solve {condition.GetName()}");
            List<PathNode> actionPaths = Paths(action);
            //Debug.Log($"Evaluating {actionPaths.Count} paths to solve {action.GetName()}");
            foreach (PathNode actionPath in actionPaths)
            {
                PathNode condPath = new PathNode(condition);
                condPath.children = new List<PathNode> { actionPath };
                condPaths.Add(condPath);
            }
            //Debug.Log($"Total paths for {condition.GetName()}: {condPaths.Count}");
        }
        return condPaths;
    }

    // Returns a new list of paths, where every path ends with the given action
    static List<PathNode> Paths(Action action)
    {
        if (action.conditions.Count == 0)
        {
            return new List<PathNode>() { new PathNode(action) };
        }

        List<PathNode> actionPaths = new List<PathNode>();
        List<List<PathNode>> condPaths = new List<List<PathNode>>();
        foreach (Condition cond in action.conditions)
        {
            condPaths.Add(Paths(cond));
        }
        IEnumerable<IEnumerable<PathNode>> combinations = CartesianProduct(condPaths);
        foreach (IEnumerable<PathNode> combin in combinations)
        {
            PathNode actionPath = new PathNode(action);
            actionPath.children = combin.ToList();
            actionPaths.Add(actionPath);
        }
        return actionPaths;
    }

    public static List<Node> DFS(Node root)
    {
        List<Node> visitOrder = new List<Node>();
        Stack<Node> visitNext = new Stack<Node>();
        visitNext.Push(root);
        while (visitNext.Count() > 0)
        {
            Node node = visitNext.Pop();
            visitOrder.Add(node);
            foreach (Node child in node.GetChildren())
            {
                visitNext.Push(child);
            }
        }
        visitOrder.Reverse();
        return visitOrder;
    }

    static IReadOnlyDictionary<int, IEnumerable<Inequality>> GenerateInequalities(IReadOnlyWorld underlying, PathNode root)
    {
        World simulated = new World(underlying);
        List<Node> dfs = DFS(root);

        Dictionary<int, IEnumerable<Inequality>> inequalities = new Dictionary<int, IEnumerable<Inequality>>();
        foreach (Node node in dfs)
        {
            PathNode pathNode = (PathNode)node;
            Node linked = pathNode.linked;
            //Debug.Log(linked.ToString());
            if (linked is Condition linkedCond)
            {
                List<Inequality> nodeIneqs = linkedCond.GenerateInequalities(simulated);
                //foreach (Inequality ineq in nodeIneqs)
                //{
                //    Debug.Log(ineq);
                //}
                inequalities.Add(node.ID, nodeIneqs);
                //if (nodeIneqs.Count > 0)
                //{
                //    pathNode.displayValues.Add("Inequalities", string.Join(", ", nodeIneqs.Select(x => x.ToString())));
                //}
            }
            else if (linked is Action linkedAction)
            {
                linkedAction.Execute(simulated);
            }
        }
        return inequalities;
    }

    static IReadOnlyDictionary<int, float> CalculateCosts(IReadOnlyWorld underlying, PathNode root, IReadOnlyDictionary<int, int> varDict) 
    {
        World simulated = new World(underlying);
        List<Node> dfs = DFS(root);

        Dictionary<int, float> costs = new Dictionary<int, float>();
        foreach (Node node in dfs)
        {
            PathNode pathNode = (PathNode)node;
            Node linked = pathNode.linked;
            if (linked is Condition linkedCond)
            {
                if (!linkedCond.SatisfiedSolved(simulated, varDict)) {
                    Debug.LogWarning($"Condition not solved {linkedCond}");
                }
                //if (nodeIneqs.Count > 0)
                //{
                //    pathNode.displayValues.Add("Inequalities", string.Join(", ", nodeIneqs.Select(x => x.ToString())));
                //}
            }
            else if (linked is Action linkedAction)
            {
                float cost = linkedAction.CalculateCost(simulated, varDict);
                ActionProgress progress = linkedAction.ExecuteSolved(simulated, varDict, null, maxTimePerAction);
                if (!progress.complete) {
                    Debug.LogWarning($"Action failed to complete {linkedAction} | estimated {linkedAction.EstimateTime(simulated, varDict)} | progress {progress}");
                    cost = float.PositiveInfinity;
                }

                costs.Add(node.ID, cost);

            }
        }
        return costs;

    }



    /// <summary>
    /// Generates every path of actions that will satisfy the given condition (with already built tree).
    /// Does not check for validity or determine cost (no simulation). 
    /// </summary>
    /// <param name="condition">Root of tree</param>
    /// <returns></returns>
    //public static List<List<Action>> ActionPaths(Condition condition)
    //{
    //    List<List<Action>> paths = new List<List<Action>>();
    //    foreach (Action action in condition.actions)
    //    {
    //        List<List<Action>> subset = ActionPaths(action);
    //        paths.AddRange(subset);
    //    }
    //    return paths;
    //}

    /// <summary>
    /// Generates every path of actions that will lead up to and include the current action.
    /// Does not check for validity or determine cost (no simulation). 
    /// </summary>
    /// <param name="action">Root of tree</param>
    /// <returns></returns>
    //public static List<List<Action>> ActionPaths(Action action)
    //{
    //    // condPaths[i] = the list of paths to satisfy actions.conditions[i]
    //    List<List<Action>>[] condPaths = new List<List<Action>>[action.conditions.Count];
    //    for (int i = 0; i < action.conditions.Count; i++)
    //    {
    //        condPaths[i] = ActionPaths(action.conditions[i]);
    //    }

    //    // combinedSequences[i] = List of lists of actions, to be executed sequentially
    //    IEnumerable<IEnumerable<List<Action>>> combinedSequences = CartesianProduct(condPaths);

    //    // combinedPaths[i] = List of actions
    //    List<List<Action>> combinedPaths = new List<List<Action>>();
    //    foreach (IEnumerable<List<Action>> sequence in combinedSequences)
    //    {
    //        List<Action> path = new List<Action>();
    //        foreach (List<Action> component in sequence)
    //        {
    //            path.AddRange(component);
    //        }
    //        combinedPaths.Add(path);
    //    }

    //    return combinedPaths;
    //}

    static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
    {
        IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
        IEnumerable<IEnumerable<T>> result = emptyProduct;
        foreach (IEnumerable<T> sequence in sequences)
        {
            result = from accseq in result from item in sequence select accseq.Concat(new[] { item });
        }
        return result;
    }
}
