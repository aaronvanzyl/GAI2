using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReadOnlyItem
{
    public string name { get; }
    public int value { get; }
    public IReadOnlyDictionary<ItemAttribute, float> readOnlyAttributes { get; }
    public Item Clone();
}
