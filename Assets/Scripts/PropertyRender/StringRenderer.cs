using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StringRenderer : PropertyRenderer
{
    public Text strText;
    public string str;

    public override void Render(IReadOnlyWorld world, IReadOnlyDictionary<int, int> varDict)
    {
        base.Render(world, varDict);
        strText.text = str;
    }
}
