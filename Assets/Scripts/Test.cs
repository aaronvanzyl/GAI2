using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public NodeTreeRenderer treeRenderer;
    World world = new World();

    void Start()
    {
        Item fish = new Item()
        {
            name = "Fish",
            value = 2,
            tags = new List<ItemTag>() { ItemTag.saleItem }
        };
        int fishID = world.RegisterItem(fish);

        Character actor = new Character()
        {
            name = "Actor",
            money = new Expression(7),
            pos = new Vector2Int(0, 0)
        };
        world.AddEntity(actor);

        Character merchant = new Character()
        {
            name = "Merchant",
            money = new Expression(100),
            pos = new Vector2Int(5, 0),
            canBuyFrom=true,
            canSellTo=true
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
        PathNode testPath = solutionTrees[1];
        List<Inequality> inequalities = PlanGenerator.GenerateInequalities(world, testPath);

        print($"found {solutionTrees.Count} solutions");
        foreach(Inequality ineq in inequalities)
        {
            print(ineq);
        }

        //treeRenderer.RenderTree(ownItemCond);
        treeRenderer.RenderTree(solutionTrees[1]);
    }

}
