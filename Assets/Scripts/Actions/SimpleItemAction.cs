using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimpleItemAction : Action
{
    public Expression amount;
    public int giverID;
    public int itemID;

    public SimpleItemAction(int actorID, int giverID, int itemID) {
        this.actorID = actorID;
        this.giverID = giverID;
        this.itemID = itemID;
        this.amount = Expression.UniqueExpression();
    }

    protected void ExecuteHelper(World world, Expression resolvedAmount) {
        Entity actor = world.GetEntity(actorID);
        actor.AddItem(itemID, resolvedAmount);
    }

    public override void Execute(World world) {
        ExecuteHelper(world, amount);
    }
    public override void ExecuteSolved(World world, Dictionary<int, int> variables) {
        ExecuteHelper(world, amount.EvaluateToExpression(variables));
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

    public override void AddProperties(NodeRenderer renderer)
    {
        base.AddProperties(renderer);
        renderer.AddEntityIDProp("Giver", giverID);
        renderer.AddItemIDProp("Item", itemID);
        renderer.AddExpressionProp("Amount", amount);
    }
}
