using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public abstract class Node
{
    public int ID = -1; // id is unique within frame (separate action trees may duplicate ids - pathnodes may duplicate id with underlying nodes)
    public int width; // Number of leaf nodes below this node

    public virtual string GetName() {
        return GetType().ToString();
    }

    public virtual void AddPropertiesTo(PropertyGroupRenderer renderer) {
        
        //renderer.canvas.sizeDelta *= new Vector2(width, 1);

    }

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
