using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathNode : Node
{
    public List<PathNode> children = new List<PathNode>();
    public Node linked;
    public KVList<string, string> displayValues = new KVList<string, string>();

    public PathNode(Node linked)
    {
        this.linked = linked;
    }


    public PathNode DeepClone()
    {
        PathNode clone = new PathNode(linked);
        foreach (PathNode child in children)
        {
            clone.children.Add(child.DeepClone());
        }
        return clone;
    }

    public override IEnumerable<Node> GetChildren()
    {
        return children;
    }

    public override string GetName()
    {
        return linked.GetName();
    }

    public override KVList<string, string> GetDisplayValues()
    {
        KVList<string, string> combined = new KVList<string, string>();
        foreach (var x in linked.GetDisplayValues())
        {
            combined.Add(x);
        }
        foreach (var x in displayValues)
        {
            combined.Add(x);
        }

        return combined;
    }
}