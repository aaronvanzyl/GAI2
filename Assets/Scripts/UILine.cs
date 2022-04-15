using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILine : MonoBehaviour
{
    public void Connect(Vector2 a, Vector2 b) {
        transform.position = (a + b) / 2;
        transform.up = (b - a);
        transform.localScale *= new Vector2(1, (b - a).magnitude);
    }
}
