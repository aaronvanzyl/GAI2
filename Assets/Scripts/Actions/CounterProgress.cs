using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterProgress : ActionProgress
{
    public int count;
    public CounterProgress(bool complete, float unusedTime, int count)
    {
        this.complete = complete;
        this.unusedTime = unusedTime;
        this.count = count;
    }
}
