using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSpot : Entity
{
    public List<int> fishingItems = new List<int>();

    public override List<Action> GenerateItemActions(int actorID, int itemID)
    {
        if (fishingItems.Contains(itemID)) {
            Action fishAction = new FishAction(actorID, ID, itemID);
            return new List<Action>() { fishAction };
        }
        return new List<Action>();
    }

    public override Entity Clone() {
        List<int> fishingItemsClone = new List<int>(fishingItems);
        FishingSpot clone = new FishingSpot();
        clone.CopyFrom(this);
        clone.fishingItems = fishingItemsClone;
        return clone;
    }
}
