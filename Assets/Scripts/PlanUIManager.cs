using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanUIManager : MonoBehaviour
{
    public PropertyGroupRenderer nodeRendererPrefab;
    public UILine linePrefab;

    public Text title;
    public Text totalCost;
    public Text totalNodes;
    public Slider planNumSlider;

    public bool alignHorizontal = true;
    public float nodeSpacingX = 1;
    public float nodeSpacingY = 1;
    public float nodeHeight = 0.5f;
    public float nodeWidth = 1f;
    public bool center = false;
    public bool drawLines = true;

    public bool showingFullTree = false;
    public int selectedPlan = 0;
    PlanSet planSet;

    public void SetPlanSet(PlanSet set) {
        planSet = set;
        selectedPlan = 0;
        showingFullTree = true;
        planNumSlider.value = 0;
        planNumSlider.maxValue = set.plans.Count - 1;
        UpdateUI();
    }

    public void SetSelectedPlan(float newPlan) {
        selectedPlan = (int)newPlan;
        UpdateUI();
    }

    public void SetShowFullTree(bool showFullTree) {
        showingFullTree = showFullTree;
        UpdateUI();
    }

    public void ToggleShowFullTree()
    {
        showingFullTree = !showingFullTree;
        UpdateUI();
    }

    void UpdateUI() {
        Clear();
        if (showingFullTree)
        {
            title.text = "Full Tree";
            totalCost.text = $"Valid Plans: {planSet.validPlans.Count}/{planSet.plans.Count}";
            totalNodes.text = "Nodes: " + PlanGenerator.DFS(planSet.cond).Count;
            RenderTree(planSet.cond, planSet.underlying, null, null, null);
        }
        else {
            Plan plan = planSet.plans[selectedPlan];
            title.text = $"Plan #{selectedPlan+1}/{planSet.plans.Count}";
            if (plan.valid)
            {
                totalCost.text = "Cost: " + plan.totalCost;
            }
            else {
                totalCost.text = "Could not solve";
            }
            totalNodes.text = "Nodes: " + plan.traverseOrder.Count;
            RenderTree(plan.root, planSet.underlying, plan.varDict, plan.ineqs, plan.costs);
        }
    }

    void RenderTree(Node rootNode, IReadOnlyWorld world, IReadOnlyDictionary<int, int> varDict = null, IReadOnlyDictionary<int, IEnumerable<Inequality>> ineqs = null, IReadOnlyDictionary<int, float> costs = null)
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
        for (int i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }
        
    }
}
