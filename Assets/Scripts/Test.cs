using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public PlanUIManager planUIManager;
    World world = new World();

    void Start()
    {
        TestWorld();
        //TestIneqSolver();
    }

    void TestIneqSolver() {
        Expression a = new Expression(0);
        a.coefficients.Add(0, 1);

        Expression b = new Expression(1);
        b.coefficients.Add(1, 1);

        // <0> >= <1> + 1
        Inequality A_geq_B = new Inequality(a, b, Comparator.GEQ);

        List<Inequality> ineqs = new List<Inequality>() { A_geq_B };
        if (IneqSolver.Solve(ineqs, out Dictionary<int, int> varDict)) {
            foreach (var kvPair in varDict) {
                Debug.Log(kvPair);
            }
        }
        else {
            Debug.Log("Unable to solve!!");
        }
    }

    void TestWorld() {

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
            money = new Expression(7),
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
            fishingItems = new List<int>() { fishID }
        };
        world.AddEntity(fishingSpot);

        OwnItemCondition ownItemCond = new OwnItemCondition(actor.ID, fishID, new Expression(3));


        PlanGenerator.GenerateTree(world, actor.ID, ownItemCond, 5, 3);
        List<PathNode> solutionTrees = PlanGenerator.Paths(ownItemCond);
        List<PathNode> solvedSolutionTrees = new List<PathNode>();
        List<Dictionary<int, int>> varDicts = new List<Dictionary<int, int>>();
        //foreach(PathNode n in solutionTrees)
        //{
        //    List<Inequality> inequalities = PlanGenerator.GenerateInequalities(world, n);
        //    if (IneqSolver.Solve(inequalities, out Dictionary<int, int> varDict)) {
        //        solvedSolutionTrees.Add(n);
        //        varDicts.Add(varDict);
        //    }
        //}

        PathNode n = solutionTrees[1]; 
        List<Inequality> inequalities = PlanGenerator.GenerateInequalities(world, n);
        if (IneqSolver.Solve(inequalities, out Dictionary<int, int> varDict))
        {
            solvedSolutionTrees.Add(n);
            varDicts.Add(varDict);
        }


        print($"found {solvedSolutionTrees.Count}/{solutionTrees.Count} valid solutions");
        //foreach (Inequality ineq in inequalities)
        //{
        //    print(ineq);
        //}

        //treeRenderer.RenderTree(ownItemCond);
        planUIManager.RenderTree(solutionTrees[1], world, varDict);
    }

}
