using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{


    public override Entity Clone()
    {
        Character clone = new Character();
        clone.CopyFrom(this);
        return clone;
    }
}
