using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PropertyRenderer : MonoBehaviour
{
    public PlanUIManager manager;
    public Text propNameText;
    public string propName;
    public virtual void Render() {
        propNameText.text = propName;
    }
}
