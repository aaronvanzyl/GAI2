using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ItemTimePair 
{
    public int itemID;
    public float timePerItem;

    public ItemTimePair(int itemID, float timePerItem)
    {
        this.itemID = itemID;
        this.timePerItem = timePerItem;
    }
}
