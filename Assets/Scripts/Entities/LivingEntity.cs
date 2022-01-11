using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : Entity
{
    public override Entity Clone()
    {
        LivingEntity clone = new LivingEntity();
        clone.CopyFrom(this);
        return clone;
    }
}
