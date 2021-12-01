using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class NodeRenderer : MonoBehaviour
{

    public Text header;
    public Text body;
    Node node;
    public RectTransform canvas;

    public void SetNode(Node node) {
        this.node = node;
        header.text = node.GetName();
        body.text = "";
        //body.text += node.GetChildren().Count() + "\n";
        //body.text += node.width + "\n";
        //print(node.GetDisplayValues().Values.Count());
        foreach (var entry in node.GetDisplayValues()) {
            body.text += entry.Key + ": " + entry.Value + "\n";
        }
        canvas.sizeDelta *= new Vector2(node.width, 1);
    }
}
