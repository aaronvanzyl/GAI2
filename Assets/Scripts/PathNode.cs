using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// PathNodes are a specific path through the complete node tree
// A PathNode may be reused in multiple paths
// Action PathNode has many condition children
// Condition PathNode has exactly one action


public class PathNode : Node
{
    public List<PathNode> children = new List<PathNode>();
    public Node linked;

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

    //public override KVList<string, string> GetDisplayValues()
    //{
    //    KVList<string, string> combined = new KVList<string, string>();
    //    foreach (var x in linked.GetDisplayValues())
    //    {
    //        combined.Add(x);
    //    }
    //    foreach (var x in displayValues)
    //    {
    //        combined.Add(x);
    //    }

    //    return combined;
    //}

    public override void AddPropertiesTo(PropertyGroupRenderer renderer)
    {
        linked.AddPropertiesTo(renderer);
    }
}