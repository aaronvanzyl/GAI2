using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Condition: Node
{
    public List<Action> actions = new List<Action>();

    public abstract List<Action> GenerateActions(IReadOnlyWorld world, int actorID, int maxActions);

    public abstract bool SatisfiedSolved(IReadOnlyWorld world, Dictionary<int, int> variables);

    public override IEnumerable<Node> GetChildren()
    {
        return actions;
    }

    public override Dictionary<string, string> GetDisplayValues()
    {
        Dictionary<string, string> displayValues = new Dictionary<string, string>();
        return displayValues;
    }

    public virtual List<Inequality> GenerateInequalities(IReadOnlyWorld world) => new List<Inequality>();
}
