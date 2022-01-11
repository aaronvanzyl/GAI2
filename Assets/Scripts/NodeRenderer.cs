using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class NodeRenderer : MonoBehaviour
{

    public Text header;
    public RectTransform canvas;
    public Transform propertyGroup;
    [HideInInspector]
    public PlanUIManager manager;
    public EntityIDRenderer entityIDRendererPrefab;
    public ItemIDRenderer itemIDRendererPrefab;
    public ExpressionRenderer expressionRendererPrefab;
    List<PropertyRenderer> propRenderers = new List<PropertyRenderer>();

    public void SetNode(Node node)
    {
        header.text = node.GetName();
        canvas.sizeDelta *= new Vector2(node.width, 1);
    }

    public void AddEntityIDProp(string propName, int entityID) {
        EntityIDRenderer prop = Instantiate(entityIDRendererPrefab, propertyGroup);
        propRenderers.Add(prop);
        prop.propName = propName;
        prop.manager = manager;
        prop.entityID = entityID;
        prop.Render();
    }

    public void AddItemIDProp(string propName, int itemID)
    {
        ItemIDRenderer prop = Instantiate(itemIDRendererPrefab, propertyGroup);
        propRenderers.Add(prop);
        prop.propName = propName;
        prop.manager = manager;
        prop.itemID = itemID;
        prop.Render();
    }

    public void AddExpressionProp(string propName, IReadOnlyExpression expression)
    {
        ExpressionRenderer prop = Instantiate(expressionRendererPrefab, propertyGroup);
        propRenderers.Add(prop);
        prop.propName = propName;
        prop.manager = manager;
        prop.expression = expression;
        prop.Render();
    }
}
