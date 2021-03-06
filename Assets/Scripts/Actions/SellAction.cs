using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellAction : Action
{
    public int merchantID;
    public int itemID;
    public Expression amount;

    public SellAction(int actorID, int merchantID, int itemID)
    {
        this.actorID = actorID;
        this.merchantID = merchantID;
        this.itemID = itemID;
        this.amount = Expression.UniqueExpression();
    }

    void ExecuteHelper(World world, Expression resolvedAmount)
    {
        Entity actor = world.GetEntity(actorID);
        Entity merchant = world.GetEntity(merchantID);

        merchant.QuerySellPrice(world, itemID, actorID, out int price);
        Expression resolvedNetPrice = resolvedAmount * price;

        actor.money += resolvedNetPrice;
        merchant.money -= resolvedNetPrice;
        actor.ReduceItem(itemID, resolvedAmount);
        merchant.AddItem(itemID, resolvedAmount);
    }

    public override void Execute(World world)
    {
        ExecuteHelper(world, amount);
    }

    public override ActionProgress ExecuteSolved(World world, IReadOnlyDictionary<int, int> variables, ActionProgress progress, float time)
    {
        ExecuteHelper(world, amount.EvaluateToExpression(variables));
        return new ActionProgress(true, time);
    }

    public override List<Condition> GenerateConditions(IReadOnlyWorld world)
    {
        IReadOnlyEntity merchant = world.GetReadOnlyEntity(merchantID);

        merchant.QuerySellPrice(world, itemID, actorID, out int price);

        CanSellCondition canSellCond = new CanSellCondition(actorID, merchantID, itemID);
        OwnItemCondition actorItemCond = new OwnItemCondition(actorID, itemID, amount);
        OwnMoneyCondition moneyCond = new OwnMoneyCondition(merchantID, amount * price);
        PosCondition posCond = new PosCondition(actorID, merchantID);

        List<Condition> conds = new List<Condition>() { canSellCond, actorItemCond, moneyCond, posCond };
        return conds;
    }

    public override Action Clone()
    {
        return new SellAction(actorID, merchantID, itemID);
    }
    //public override KVList<string, string> GetDisplayValues()
    //{
    //    KVList<string, string> displayValues = base.GetDisplayValues();
    //    displayValues.Add("Merchant ID", merchantID.ToString());
    //    displayValues.Add("Item ID", itemID.ToString());
    //    displayValues.Add("Amount", amount.ToString());
    //    return displayValues;
    //}

    public override void AddPropertiesTo(PropertyGroupRenderer renderer)
    {
        base.AddPropertiesTo(renderer);
        renderer.AddEntityIDProp("Merchant", merchantID);
        renderer.AddItemIDProp("Item", itemID);
        renderer.AddExpressionProp("Amount", amount);
    }

    public override float CalculateCost(IReadOnlyWorld world, IReadOnlyDictionary<int, int> variables)
    {
        IReadOnlyEntity actor = world.GetReadOnlyEntity(actorID);
        IReadOnlyEntity merchant = world.GetReadOnlyEntity(merchantID);

        int resolvedAmount = amount.Evaluate(variables);
        merchant.QueryBuyPrice(world, itemID, actorID, out int price);
        int resolvedPrice = resolvedAmount * price;
        float cost = 0;
        cost += actor.MoneyChangeCost(resolvedPrice);
        cost += actor.ItemChangeCost(world, itemID, -resolvedAmount);
        return cost;
    }

    public override float EstimateTime(IReadOnlyWorld world, IReadOnlyDictionary<int, int> variables)
    {
        return 0;
    }
}
