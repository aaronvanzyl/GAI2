using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosCondition : Condition
{
    public int moverID;
    public int targetID;

    public PosCondition(int moverID, int targetID)
    {
        this.moverID = moverID;
        this.targetID = targetID;
    }

    public override List<Action> GenerateActions(IReadOnlyWorld world, int actorID, int max)
    {
        if (actorID == moverID) {
            List<Action> actions = new List<Action>();
            actions.Add(new MoveAction(moverID, targetID));
            return actions;
        }
        Debug.Log($"Attempting to move non-actor {moverID} {actorID}");
        return new List<Action>();
    }

    public override bool SatisfiedSolved(IReadOnlyWorld world, IReadOnlyDictionary<int, int> variables)
    {
        return world.GetReadOnlyEntity(moverID).pos == world.GetReadOnlyEntity(targetID).pos;
    }

    //public override KVList<string, string> GetDisplayValues()
    //{
    //    KVList<string, string> displayValues = base.GetDisplayValues();
    //    displayValues.Add("Mover ID", moverID.ToString());
    //    displayValues.Add("Target ID", targetID.ToString());
    //    return displayValues;
    //}

    public override void AddPropertiesTo(PropertyGroupRenderer renderer)
    {
        base.AddPropertiesTo(renderer);
        renderer.AddEntityIDProp("Mover", moverID);
        renderer.AddEntityIDProp("Target", targetID);
    }
}
