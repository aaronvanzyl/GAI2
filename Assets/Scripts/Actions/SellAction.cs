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
        Expression resolvedPrice = resolvedAmount * price;

        actor.money += resolvedPrice;
        merchant.money -= resolvedPrice;
        actor.ReduceItem(itemID, resolvedAmount);
        merchant.AddItem(itemID, resolvedAmount);
    }

    public override void Execute(World world)
    {
        ExecuteHelper(world, amount);
    }

    public override void ExecuteSolved(World world, Dictionary<int, int> variables)
    {
        ExecuteHelper(world, amount.EvaluateToExpression(variables));
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
    public override Dictionary<string, string> GetDisplayValues()
    {
        Dictionary<string, string> displayValues = base.GetDisplayValues();
        displayValues.Add("Merchant ID", merchantID.ToString());
        displayValues.Add("Item ID", itemID.ToString());
        displayValues.Add("Amount", amount.ToString());
        return displayValues;
    }

}