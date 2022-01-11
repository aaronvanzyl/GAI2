using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemIDRenderer : PropertyRenderer
{
    public Text itemIDText;
    public int itemID;

    public override void Render()
    {
        base.Render();
        IReadOnlyItem item = manager.world.GetItem(itemID);
        itemIDText.text = $"{item.name} <{itemID}>";
    }
}
