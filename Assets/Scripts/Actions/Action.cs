using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Action: Node
{

    public int actorID;
    public List<Condition> conditions;

    public abstract void Execute(World world);
    public abstract ActionProgress ExecuteSolved(World world, Dictionary<int, int> variables, ActionProgress progress, float time);
    public abstract float CalcCost(IReadOnlyWorld world, Dictionary<int, int> variables);
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

    public override void AddProperties(NodeRenderer renderer)
    {
        base.AddProperties(renderer);
        renderer.AddEntityIDProp("Actor", actorID);
    }
}
