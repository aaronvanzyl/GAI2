using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSellCondition : Condition
{
    int sellerID;
    int merchantID;
    int itemID;

    public CanSellCondition(int sellerID, int merchantID, int itemID)
    {
        this.sellerID = sellerID;
        this.merchantID = merchantID;
        this.itemID = itemID;
    }

    public override List<Action> GenerateActions(IReadOnlyWorld world, int actorID, int max)
    {
        return new List<Action>();
    }

    public override bool SatisfiedSolved(IReadOnlyWorld world, Dictionary<int, int> variables)
    {
        return world.GetReadOnlyEntity(merchantID).QuerySellPrice(world, itemID, sellerID, out _);
    }

    public override KVList<string, string> GetDisplayValues()
    {
        KVList<string, string> displayValues = base.GetDisplayValues();
        displayValues.Add("Merchant ID", merchantID.ToString());
        displayValues.Add("Seller ID", sellerID.ToString());
        displayValues.Add("Item ID", itemID.ToString());
        return displayValues;
    }
}
