using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Condition: Node
{
    public List<Action> actions = new List<Action>();

    public abstract List<Action> GenerateActions(IReadOnlyWorld world, int actorID, int maxActions);

    public abstract bool SatisfiedSolved(IReadOnlyWorld world, IReadOnlyDictionary<int, int> variables);

    public override IEnumerable<Node> GetChildren()
    {
        return actions;
    }

    //public override KVList<string, string> GetDisplayValues()
    //{
    //    KVList<string, string> displayValues = new KVList<string, string>();
    //    return displayValues;
    //}

    public override void AddPropertiesTo(PropertyGroupRenderer renderer)
    {
        base.AddPropertiesTo(renderer);
    }

    public virtual List<Inequality> GenerateInequalities(IReadOnlyWorld world) => new List<Inequality>();
}
