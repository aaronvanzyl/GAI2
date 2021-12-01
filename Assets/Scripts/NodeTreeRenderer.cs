using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeTreeRenderer : MonoBehaviour
{
    public NodeRenderer nodeRendererPrefab;
    public bool alignHorizontal = true;
    public float nodeWidth = 1;
    public float nodeHeight = 1;
    public bool center = false;

    public void RenderTree(Node rootNode)
    {
        rootNode.CalculateWidthRecursive();
        RenderTreeRecursive(rootNode, Vector3.zero);
    }

    void RenderTreeRecursive(Node node, Vector3 position)
    {
        //Debug.Log($"rendering node: in: {children} out: {node.outNode != null}");
        NodeRenderer renderer = Instantiate(nodeRendererPrefab, position, Quaternion.identity, transform);
        renderer.SetNode(node);
        //renderer.transform.localScale = alignHorizontal ? new Vector3(1, node.width, 1) : new Vector3(node.width, 1, 1);

        float offset = center ? -node.width * 0.5f : 0;
        foreach (Node child in node.GetChildren())
        {
            float mainAxisOffset = alignHorizontal ? -nodeWidth : -nodeHeight;
            float orthogonalOffset = 0;
            if (center)
            {
                orthogonalOffset = (offset + child.width * 0.5f) * (alignHorizontal ? nodeHeight : nodeWidth);
            }
            else {
                orthogonalOffset = offset * (alignHorizontal ? nodeHeight : nodeWidth);
            }
             
            //print($"ortho offset {orthogonalOffset}");
            Vector3 childPos = new Vector3(position.x + (alignHorizontal ? mainAxisOffset : orthogonalOffset), position.y + (alignHorizontal ? orthogonalOffset : mainAxisOffset), position.z);
            RenderTreeRecursive(child, childPos);
            offset += child.width;
        }
    }

    public void Clear()
    {

    }
}
