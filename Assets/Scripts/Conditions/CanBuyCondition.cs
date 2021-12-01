using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBuyCondition : Condition
{
    int buyerID;
    int merchantID;
    int itemID;

    public CanBuyCondition(int buyerID, int merchantID, int itemID)
    {
        this.buyerID = buyerID;
        this.merchantID = merchantID;
        this.itemID = itemID;
    }

    public override List<Action> GenerateActions(IReadOnlyWorld world, int actorID, int max)
    {
        return new List<Action>();
    }

    public override bool SatisfiedSolved(IReadOnlyWorld world, Dictionary<int, int> variables)
    {
        return world.GetReadOnlyEntity(merchantID).QueryBuyPrice(world, itemID, buyerID, out _);
    }

    public override Dictionary<string, string> GetDisplayValues()
    {
        Dictionary<string, string> displayValues = base.GetDisplayValues();
        displayValues.Add("Merchant ID", merchantID.ToString());
        displayValues.Add("Buyer ID", buyerID.ToString());
        displayValues.Add("Item ID", itemID.ToString());
        return displayValues;
    }
}
