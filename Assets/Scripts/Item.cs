using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : IReadOnlyItem
{
    public string name { get; set; }
    public int value { get; set; }
    public Dictionary<ItemAttribute, float> attributes;
    public IReadOnlyDictionary<ItemAttribute, float> readOnlyAttributes { get => attributes; }


    public Item Clone()
    {
        return new Item()
        {
            name = name,
            value = value,
            attributes = new Dictionary<ItemAttribute, float>(attributes)
        };
    }



    //public static List<int> SatisfyFilter(ItemFilter filter) {
    //    List<int> IDs = new List<int>();
    //    foreach (KeyValuePair<int, IReadOnlyItem> entry in itemDict) {
    //        if (filter.Satisfied(entry.Value)) {
    //            IDs.Add(entry.Key);
    //        }
    //    }
    //    return IDs;
    //}

}
