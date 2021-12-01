using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReadOnlyItem
{
    public string name { get; }
    public int value { get; }
    public IReadOnlyList<ItemTag> readOnlyTags { get; }
    public Item Clone();
}
