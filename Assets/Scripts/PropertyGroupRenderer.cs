using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PropertyGroupRenderer : MonoBehaviour
{

    public Text header;
    public RectTransform canvas;
    public Transform propertyGroup;
    [HideInInspector]
    public EntityIDRenderer entityIDRendererPrefab;
    public ItemIDRenderer itemIDRendererPrefab;
    public ExpressionRenderer expressionRendererPrefab;
    public InequalityRenderer ineqRendererPrefab;
    public StringRenderer strRendererPrefab;
    List<PropertyRenderer> propRenderers = new List<PropertyRenderer>();

    public void SetHeaderText(string text) {
        header.text = text;
    }

    public void AddEntityIDProp(string propName, int entityID) {
        EntityIDRenderer prop = Instantiate(entityIDRendererPrefab, propertyGroup);
        propRenderers.Add(prop);
        prop.propName = propName;
        prop.entityID = entityID;
    }

    public void AddItemIDProp(string propName, int itemID)
    {
        ItemIDRenderer prop = Instantiate(itemIDRendererPrefab, propertyGroup);
        propRenderers.Add(prop);
        prop.propName = propName;
        prop.itemID = itemID;
    }

    public void AddExpressionProp(string propName, IReadOnlyExpression expression)
    {
        ExpressionRenderer prop = Instantiate(expressionRendererPrefab, propertyGroup);
        propRenderers.Add(prop);
        prop.propName = propName;
        prop.expression = expression;
    }

    public void AddInequalityProp(string propName, Inequality ineq) {
        InequalityRenderer prop = Instantiate(ineqRendererPrefab, propertyGroup);
        propRenderers.Add(prop);
        prop.propName = propName;
        prop.inequality = ineq;
    }

    public void AddStringProp(string propName, string str) {
        StringRenderer prop = Instantiate(strRendererPrefab, propertyGroup);
        propRenderers.Add(prop);
        prop.str = str;
    }

    public void Render(IReadOnlyWorld world, IReadOnlyDictionary<int, int> varDict)
    {
        foreach (PropertyRenderer prop in propRenderers) {
            prop.Render(world, varDict);
        }
    }
}
