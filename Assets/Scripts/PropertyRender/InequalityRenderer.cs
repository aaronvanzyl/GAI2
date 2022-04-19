using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InequalityRenderer : PropertyRenderer
{
    public Inequality inequality;
    public Text inequalityText;

    public override void Render(IReadOnlyWorld world, IReadOnlyDictionary<int, int> varDict)
    {
        base.Render(world, varDict);
        inequalityText.text = inequality.ToString();
    }
}
