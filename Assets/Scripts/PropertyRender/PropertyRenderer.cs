using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PropertyRenderer : MonoBehaviour
{
    public Text propNameText;
    public string propName;
    public virtual void Render(IReadOnlyWorld world, IReadOnlyDictionary<int, int> varDict) {
        propNameText.text = propName;
    }
}
