using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemIDRenderer : PropertyRenderer
{
    public Text itemIDText;
    public int itemID;

    public override void Render(IReadOnlyWorld world, IReadOnlyDictionary<int, int> varDict)
    {
        base.Render(world, varDict);
        IReadOnlyItem item = world.GetItem(itemID);
        itemIDText.text = $"{item.name} <{itemID}>";
    }
}
