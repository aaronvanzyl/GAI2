using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanUIManager : MonoBehaviour
{
    public PropertyGroupRenderer nodeRendererPrefab;
    public UILine linePrefab;
    public bool alignHorizontal = true;
    public float nodeSpacingX = 1;
    public float nodeSpacingY = 1;
    public float nodeHeight = 0.5f;
    public float nodeWidth = 1f;
    public bool center = false;
    public bool drawLines = true;

    public void RenderTree(Node rootNode, IReadOnlyWorld world, IReadOnlyDictionary<int, int> varDict = null, IReadOnlyDictionary<int, IEnumerable<Inequality>> ineqs = null, IReadOnlyDictionary<int, float> costs = null)
    {
        rootNode.CalculateWidthRecursive();
        RenderTreeRecursive(rootNode, Vector3.zero, world, varDict, ineqs, costs);
    }

    void RenderTreeRecursive(Node node, Vector3 position, IReadOnlyWorld world, IReadOnlyDictionary<int, int> varDict = null, IReadOnlyDictionary<int, IEnumerable<Inequality>> ineqs = null, IReadOnlyDictionary<int, float> costs = null)
    {
        //Debug.Log($"rendering node: in: {children} out: {node.outNode != null}");
        PropertyGroupRenderer renderer = Instantiate(nodeRendererPrefab, position, Quaternion.identity, transform);
        node.AddPropertiesTo(renderer);
        renderer.header.text = $"{node.GetName()} [{node.ID}]";
        if (costs != null && costs.TryGetValue(node.ID, out float cost))
        {
            renderer.AddStringProp("Cost", $"{cost:.3f}");
        }
        if (ineqs != null && ineqs.TryGetValue(node.ID, out IEnumerable<Inequality> nodeIneqs)) {
            foreach (Inequality ineq in nodeIneqs) {
                renderer.AddInequalityProp("Inequality", ineq);
            }
        }
        renderer.Render(world, varDict);
        //renderer.transform.localScale = alignHorizontal ? new Vector3(1, node.width, 1) : new Vector3(node.width, 1, 1);

        float offset = center ? -node.width * 0.5f : 0;
        foreach (Node child in node.GetChildren())
        {
            float mainAxisOffset = alignHorizontal ? -nodeSpacingX : -nodeSpacingY;
            float orthogonalOffset = 0;
            if (center)
            {
                orthogonalOffset = (offset + child.width * 0.5f) * (alignHorizontal ? nodeSpacingY : nodeSpacingX);
            }
            else {
                orthogonalOffset = offset * (alignHorizontal ? nodeSpacingY : nodeSpacingX);
            }
             
            //print($"ortho offset {orthogonalOffset}");
            Vector3 childPos = new Vector3(position.x + (alignHorizontal ? mainAxisOffset : orthogonalOffset), position.y + (alignHorizontal ? orthogonalOffset : mainAxisOffset), position.z);
            if (drawLines) {
                UILine line = Instantiate(linePrefab, transform);
                line.Connect(position + new Vector3(nodeWidth/2, -nodeHeight, 0), childPos + new Vector3(nodeWidth/2, 0, 0));
            }
            RenderTreeRecursive(child, childPos, world, varDict, ineqs, costs);
            offset += child.width;
        }
    }

    public void Clear()
    {

    }
}
