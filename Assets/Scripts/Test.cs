using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Test : MonoBehaviour
{
    public PlanUIManager planUIManager;
    World world = new World();

    void Start()
    {
        TestWorld();
        //TestIneqSolver();
    }

    void TestIneqSolver()
    {
        Expression a = new Expression(0);
        a.coefficients.Add(0, 1);

        Expression b = new Expression(1);
        b.coefficients.Add(1, 1);

        // <0> >= <1> + 1
        Inequality A_geq_B = new Inequality(a, b, Comparator.GEQ);

        List<Inequality> ineqs = new List<Inequality>() { A_geq_B };
        if (IneqSolver.Solve(ineqs, out Dictionary<int, int> varDict))
        {
            foreach (var kvPair in varDict)
            {
                Debug.Log(kvPair);
            }
        }
        else
        {
            Debug.Log("Unable to solve!!");
        }
    }

    void TestWorld()
    {

        Item fish = new Item()
        {
            name = "Fish",
            value = 2,
            tags = new List<ItemTag>() { ItemTag.saleItem }
        };
        int fishID = world.RegisterItem(fish);

        LivingEntity actor = new LivingEntity()
        {
            name = "Actor",
            money = new Expression(0),
            pos = new Vector2Int(0, 0)
        };
        world.AddEntity(actor);

        LivingEntity merchant = new LivingEntity()
        {
            name = "Merchant",
            money = new Expression(100),
            pos = new Vector2Int(5, 0),
            canBuyFrom = true,
            canSellTo = true
        };
        merchant.AddItem(fishID, new Expression(100));
        world.AddEntity(merchant);

        FishingSpot fishingSpot = new FishingSpot()
        {
            name = "Lake",
            pos = new Vector2Int(0, 5),
            fishingItems = new List<ItemTimePair>() { new ItemTimePair(fishID, fish.value) }
        };
        world.AddEntity(fishingSpot);

        OwnItemCondition ownItemCond = new OwnItemCondition(actor.ID, fishID, new Expression(3));


        PlanGenerator.GenerateTree(world, actor.ID, ownItemCond, 5, 3);
        List<PathNode> solutionTrees = PlanGenerator.GeneratePathTrees(ownItemCond);
        List<int> solvedSolutionTrees = new List<int>(); 
        List<IReadOnlyDictionary<int, int>> varDicts = new List<IReadOnlyDictionary<int, int>>(); 
        List<IReadOnlyDictionary<int, IEnumerable<Inequality>>> solutionTreeIneqs = new List<IReadOnlyDictionary<int, IEnumerable<Inequality>>>();
        for(int i = 0; i < solutionTrees.Count; i++)
        {
            PathNode n = solutionTrees[i];
            IReadOnlyDictionary<int, IEnumerable<Inequality>> inequalities = PlanGenerator.GenerateInequalities(world, n);
            List<Inequality> allInequalities = new List<Inequality>();
            foreach (IEnumerable<Inequality> ls in inequalities.Values) {
                allInequalities.AddRange(ls);
            }
            solutionTreeIneqs.Add(inequalities);
            if (IneqSolver.Solve(allInequalities, out Dictionary<int, int> varDict))
            {
                varDicts.Add(varDict);
                solvedSolutionTrees.Add(i);
            }
            else {
                varDicts.Add(null);
            }
        }
        //print($"found {solutionTrees.Count} solutions");
        //foreach (PathNode node in solutionTrees)
        //{
        //    print($"Tree starting with action {node.children[0].GetName()}");
        //}

        //PathNode n = solutionTrees[0]; 
        //List<Inequality> inequalities = PlanGenerator.GenerateInequalities(world, n);
        //if (IneqSolver.Solve(inequalities, out Dictionary<int, int> varDict))
        //{
        //    solvedSolutionTrees.Add(n);
        //    varDicts.Add(varDict);
        //}


        print($"found {solvedSolutionTrees.Count}/{solutionTrees.Count} valid solutions");
        //foreach (Inequality ineq in inequalities)
        //{
        //    print(ineq);
        //}

        //planUIManager.RenderTree(ownItemCond, world, new Dictionary<int, int>());
        int selectedTree = 1;
        //foreach (Node n in PlanGenerator.DFS(solutionTrees[selectedTree])) {
        //    if (solutionTreeIneqs[selectedTree].TryGetValue(n.ID, out IEnumerable<Inequality> ineqs) && ineqs.Any()) {
        //        Debug.Log(n.ID);
        //        foreach (Inequality ineq in ineqs) {
        //            Debug.Log(ineq);
        //        }
        //    }
        //}
        planUIManager.RenderTree(solutionTrees[selectedTree], world, varDicts[selectedTree], solutionTreeIneqs[selectedTree]);
    }

}
