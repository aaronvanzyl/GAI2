using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expression : IReadOnlyExpression
{
    static int maxVariableID;

    public int constant { get; set; }
    public SortedList<int, float> coefficients = new SortedList<int, float>();

    public Expression(int constant)
    {
        this.constant = constant;
    }

    public Expression(IReadOnlyExpression expr)
    {
        constant = expr.constant;
        foreach (var entry in expr.readonlyCoefficients) {
            coefficients.Add(entry.Key, entry.Value);
        }
    }

    public IReadOnlyDictionary<int, float> readonlyCoefficients => coefficients;

    public int Evaluate(Dictionary<int, int> variables)
    {
        float sum = constant;
        foreach (KeyValuePair<int, float> entry in coefficients)
        {
            sum += variables[entry.Key] * entry.Value;
        }
        return (int)sum;
    }

    public Expression EvaluateToExpression(Dictionary<int, int> variables)
    {
        return new Expression(Evaluate(variables));
    }

    public override string ToString() {
        string str = "";
        if (constant != 0)
        {
            str += constant;
        }
        foreach (KeyValuePair<int, float> kv in coefficients) {
            if (str.Length > 0) {
                str += kv.Value >= 0 ? " + " : " - ";
            }
            else if (kv.Value < 0) {
                str += "-";
            }
            if (Mathf.Abs(kv.Value) != 1) {
                str += Mathf.Abs(kv.Value);
            }
            str += "{"+kv.Key+"}";
        }
        return str;
    }

    public static Expression operator +(Expression a) => a * 1;

    public static Expression operator -(Expression a) => a * -1;

    public static Expression operator +(Expression a, Expression b)
    {
        Expression result = new Expression(0);
        result.constant = a.constant + b.constant;
        foreach (KeyValuePair<int, float> entry in a.coefficients)
        {
            result.coefficients.Add(entry.Key, entry.Value);
        }

        foreach (KeyValuePair<int, float> entry in b.coefficients)
        {
            if (result.coefficients.TryGetValue(entry.Key, out float value))
            {
                result.coefficients[entry.Key] = value + entry.Value;
            }
            else
            {
                result.coefficients[entry.Key] = entry.Value;
            }
        }
        return result;
    }

    public static Expression operator -(Expression a, Expression b) => a + (-b);

    public static Expression operator *(Expression a, float c)
    {
        Expression result = new Expression(0);
        result.constant = (int)(a.constant * c);
        foreach (KeyValuePair<int, float> entry in a.coefficients)
        {
            result.coefficients.Add(entry.Key, entry.Value * c);
        }
        return result;
    }



    public bool StrictlyGEQ(IReadOnlyExpression b)
    {
        if (!SameKeys(this, b))
        {
            return false;
        }

        if (constant < b.constant)
        {
            return false;
        }

        foreach (KeyValuePair<int, float> entry in coefficients)
        {
            if (entry.Value < b.readonlyCoefficients[entry.Key])
            {
                return false;
            }
        }

        return true;
    }

    public bool StrictlyLEQ(IReadOnlyExpression b)
    {
        if (!SameKeys(this, b))
        {
            return false;
        }

        if (constant > b.constant)
        {
            return false;
        }

        foreach (KeyValuePair<int, float> entry in coefficients)
        {
            if (entry.Value > b.readonlyCoefficients[entry.Key])
            {
                return false;
            }
        }

        return true;
    }

    public static bool SameKeys(IReadOnlyExpression a, IReadOnlyExpression b)
    {
        foreach (int key in a.readonlyCoefficients.Keys)
        {
            if (!b.readonlyCoefficients.ContainsKey(key))
            {
                return false;
            }
        }
        foreach (int key in b.readonlyCoefficients.Keys)
        {
            if (!a.readonlyCoefficients.ContainsKey(key))
            {
                return false;
            }
        }
        return true;
    }

    public static int UniqueVariableID()
    {
        maxVariableID++;
        return maxVariableID;
    }

    public static Expression UniqueExpression()
    {
        Expression expression = new Expression(0);
        expression.coefficients[UniqueVariableID()] = 1;
        return expression;
    }

    public IReadOnlyExpression Clone()
    {
        return this * 1;
    }

}
