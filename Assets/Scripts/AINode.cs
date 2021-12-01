using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public abstract class Node
{
    public int width;

    public virtual string GetName() {
        return GetType().ToString();
    }

    public abstract Dictionary<string, string> GetDisplayValues();

    public abstract IEnumerable<Node> GetChildren();

    public void CalculateWidthRecursive()
    {
        if (!GetChildren().Any())
        {
            width = 1;
        }
        else
        {
            width = 0;
            foreach (Node n in GetChildren())
            {
                n.CalculateWidthRecursive();
                width += n.width;
            }
        }
    }

}
