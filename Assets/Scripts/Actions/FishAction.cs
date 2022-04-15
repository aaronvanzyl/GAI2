using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAction : SimpleItemAction
{

    public FishAction(int actorID, int giverID, int itemID, float timePerItem) : base(actorID, giverID, itemID, timePerItem)
    {
    }

    public override Action Clone()
    {
        return new FishAction(actorID, giverID, itemID, timePerItem);
    }

    public override float EstimateTime(IReadOnlyWorld world, Dictionary<int, int> variables)
    {
        return amount.Evaluate(variables) * timePerItem;
    }

    public override float EstimateCost(IReadOnlyWorld world, Dictionary<int, int> variables)
    {
        IReadOnlyEntity actor = world.GetReadOnlyEntity(actorID);
        int resolvedAmount = amount.Evaluate(variables);
        float cost = 0;
        cost += actor.aiConfig.fishingCostMult * EstimateTime(world, variables);
        cost += actor.ItemChangeCost(world, itemID, resolvedAmount);
        return cost;
    }

}
