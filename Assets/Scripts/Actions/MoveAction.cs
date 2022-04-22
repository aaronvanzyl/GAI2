using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : Action
{
    public int targetID;

    public MoveAction(int actorID, int targetID)
    {
        this.actorID = actorID;
        this.targetID = targetID;
    }

    public override Action Clone()
    {
        return new MoveAction(actorID, targetID);
    }

    public override void Execute(World world)
    {
        world.GetEntity(actorID).pos = world.GetReadOnlyEntity(targetID).pos;
    }

    public override ActionProgress ExecuteSolved(World world, IReadOnlyDictionary<int, int> variables, ActionProgress progress, float time)
    {
        // TODO: implement movement
        Execute(world);
        return new ActionProgress(true, time);
    }

    public override List<Condition> GenerateConditions(IReadOnlyWorld world)
    {
        return new List<Condition>();
    }

    //public override KVList<string, string> GetDisplayValues()
    //{
    //    KVList<string, string> displayValues = base.GetDisplayValues();
    //    displayValues.Add("Target ID", targetID.ToString());
    //    return displayValues;
    //}

    public override void AddPropertiesTo(PropertyGroupRenderer renderer)
    {
        base.AddPropertiesTo(renderer);
        renderer.AddEntityIDProp("Target", targetID);
    }

    public override float CalculateCost(IReadOnlyWorld world, IReadOnlyDictionary<int, int> variables)
    {
        return 0;
    }

    public override float EstimateTime(IReadOnlyWorld world, IReadOnlyDictionary<int, int> variables)
    {
        throw new System.NotImplementedException();
    }
}
