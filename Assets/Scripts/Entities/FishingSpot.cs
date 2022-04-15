using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSpot : Entity
{
    public List<ItemTimePair> fishingItems = new List<ItemTimePair>();

    public override List<Action> GenerateItemActions(int actorID, int itemID)
    {
        List<Action> actions = new List<Action>();
        foreach(ItemTimePair entry in fishingItems) {
            if (entry.itemID == itemID) {
                Action fishAction = new FishAction(actorID, ID, itemID, entry.timePerItem);
                actions.Add(fishAction);
            }
        }
        return actions;
    }

    public override Entity Clone() {
        List<ItemTimePair> fishingItemsClone = new List<ItemTimePair>(fishingItems);
        FishingSpot clone = new FishingSpot();
        clone.CopyFrom(this);
        clone.fishingItems = fishingItemsClone;
        return clone;
    }
}
