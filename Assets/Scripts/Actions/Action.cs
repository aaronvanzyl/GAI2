using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Action: Node
{

    public int actorID;
    public List<Condition> conditions;

    public abstract void Execute(World world);
    public abstract ActionProgress ExecuteSolved(World world, IReadOnlyDictionary<int, int> variables, ActionProgress progress, float time);
    public abstract float CalculateCost(IReadOnlyWorld world, IReadOnlyDictionary<int, int> variables);
    public abstract float EstimateTime(IReadOnlyWorld world, IReadOnlyDictionary<int, int> variables);
    public abstract List<Condition> GenerateConditions(IReadOnlyWorld world);
    public abstract Action Clone();

    public override IEnumerable<Node> GetChildren()
    {
        return conditions;
    }

    //public override KVList<string, string> GetDisplayValues() {
    //    KVList<string, string> displayValues = new KVList<string, string>();
    //    displayValues.Add("Actor ID", actorID.ToString());
    //    return displayValues;
    //}

    public override void AddPropertiesTo(PropertyGroupRenderer renderer)
    {
        base.AddPropertiesTo(renderer);
        renderer.AddEntityIDProp("Actor", actorID);
    }
}
