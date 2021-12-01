using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : Entity
{
    public override Entity Clone()
    {
        Shop clone = new Shop();
        clone.CopyFrom(this);
        return clone;
    }
}
