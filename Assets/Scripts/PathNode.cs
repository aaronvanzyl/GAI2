using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override Dictionary<string, string> GetDisplayValues()
    {
        return linked.GetDisplayValues();   
    }
}