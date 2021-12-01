using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyAction : Action
{
    public int merchantID;
    public int itemID;
    public Expression amount;

    public BuyAction(int actorID, int merchantID, int itemID)
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

        merchant.QueryBuyPrice(world, itemID, actorID, out int price);
        Expression resolvedPrice = resolvedAmount * price;

        actor.money -= resolvedPrice;
        merchant.money += resolvedPrice;
        actor.AddItem(itemID, resolvedAmount);
        merchant.ReduceItem(itemID, resolvedAmount);
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

        merchant.QueryBuyPrice(world, itemID, actorID, out int price);

        CanBuyCondition canBuyCond = new CanBuyCondition(actorID, merchantID, itemID);
        OwnItemCondition merchantItemCond = new OwnItemCondition(merchantID, itemID, amount);
        OwnMoneyCondition moneyCond = new OwnMoneyCondition(actorID, amount * price);
        PosCondition posCond = new PosCondition(actorID, merchantID);

        List<Condition> conds = new List<Condition>() { canBuyCond, merchantItemCond, moneyCond, posCond };
        return conds;
    }

    public override Action Clone()
    {
        return new BuyAction(actorID, merchantID, itemID);
    }

    public override KVList<string, string> GetDisplayValues()
    {
        KVList<string, string> displayValues = base.GetDisplayValues();
        displayValues.Add("Merchant ID", merchantID.ToString());
        displayValues.Add("Item ID", itemID.ToString());
        displayValues.Add("Amount", amount.ToString());
        return displayValues;
    }
}
