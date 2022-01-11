using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpressionRenderer : PropertyRenderer
{
    public IReadOnlyExpression expression;
    public Text expressionText;

    public override void Render()
    {
        base.Render();
        if (manager.varDict.Count > 0 && expression.readonlyCoefficients.Count > 0) {
            expressionText.text = $"{expression.Evaluate(manager.varDict)} || {expression}";
        }
        else {
            expressionText.text = expression.ToString();
        }
    }
}
