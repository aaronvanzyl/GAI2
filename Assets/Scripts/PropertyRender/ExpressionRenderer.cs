using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpressionRenderer : PropertyRenderer
{
    public IReadOnlyExpression expression;
    public Text expressionText;

    public override void Render(IReadOnlyWorld world, IReadOnlyDictionary<int, int> varDict)
    {
        base.Render(world, varDict);
        if (expression.CanEvaluate(varDict) && expression.readonlyCoefficients.Count > 0) {
            expressionText.text = $"{expression.Evaluate(varDict)} || {expression}";
        }
        else {
            expressionText.text = expression.ToString();
        }
    }
}
