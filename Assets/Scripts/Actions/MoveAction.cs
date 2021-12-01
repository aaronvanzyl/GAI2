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
        world.GetEntity(actorID).pos = world.GetEntity(targetID).pos;
    }

    public override void ExecuteSolved(World world, Dictionary<int, int> variables)
    {
        Execute(world);
    }

    public override List<Condition> GenerateConditions(IReadOnlyWorld world)
    {
        return new List<Condition>();
    }

    public override KVList<string, string> GetDisplayValues()
    {
        KVList<string, string> displayValues = base.GetDisplayValues();
        displayValues.Add("Target ID", targetID.ToString());
        return displayValues;
    }
}
