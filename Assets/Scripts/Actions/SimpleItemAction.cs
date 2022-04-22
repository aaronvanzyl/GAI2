using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimpleItemAction : Action
{

    public Expression amount;
    public float timePerItem;
    public int giverID;
    public int itemID;

    public SimpleItemAction(int actorID, int giverID, int itemID, float timePerItem) {
        this.actorID = actorID;
        this.giverID = giverID;
        this.itemID = itemID;
        this.timePerItem = timePerItem;
        this.amount = Expression.UniqueExpression();
    }

    public override void Execute(World world) {
        Entity actor = world.GetEntity(actorID);
        actor.AddItem(itemID, amount);
    }

    public override ActionProgress ExecuteSolved(World world, IReadOnlyDictionary<int, int> variables, ActionProgress progress, float time)
    {
        int pastItemsGained = progress == null ? 0 : ((CounterProgress)progress).count;
        int itemsNeeded = amount.Evaluate(variables) - pastItemsGained;
        int itemsGained = Mathf.Min((int)(time / timePerItem), itemsNeeded);
        int totalItemsGained = itemsGained + pastItemsGained;
        float timeUsed = itemsGained * timePerItem;

        Entity actor = world.GetEntity(actorID);
        actor.AddItem(itemID, new Expression(itemsGained));

        return new CounterProgress(totalItemsGained == itemsNeeded, time - timeUsed, totalItemsGained);
    }
    public override List<Condition> GenerateConditions(IReadOnlyWorld world)
    {
        PosCondition posCond = new PosCondition(actorID, giverID);
        return new List<Condition>() { posCond };
    }

    //public override KVList<string, string> GetDisplayValues()
    //{
    //    KVList<string, string> displayValues = base.GetDisplayValues();
    //    displayValues.Add("Giver ID", giverID.ToString());
    //    displayValues.Add("Item ID", itemID.ToString());
    //    displayValues.Add("Amount", amount.ToString());
    //    return displayValues;
    //}

    public override void AddPropertiesTo(PropertyGroupRenderer renderer)
    {
        base.AddPropertiesTo(renderer);
        renderer.AddEntityIDProp("Giver", giverID);
        renderer.AddItemIDProp("Item", itemID);
        renderer.AddExpressionProp("Amount", amount);
    }
}
