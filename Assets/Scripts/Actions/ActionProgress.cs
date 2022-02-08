using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionProgress
{
    public bool complete = false;
    public float unusedTime = 0;

    public ActionProgress() { }

    public ActionProgress(bool complete, float unusedTime)
    {
        this.complete = complete;
        this.unusedTime = unusedTime;
    }
}
