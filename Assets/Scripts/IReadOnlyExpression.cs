using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReadOnlyExpression
{
    public int constant { get; }
    public IReadOnlyDictionary<int, float> readonlyCoefficients { get; }

    public bool StrictlyGEQ(IReadOnlyExpression other);
    public bool StrictlyLEQ(IReadOnlyExpression other);
    public IReadOnlyExpression Clone();
    public abstract int Evaluate(Dictionary<int, int> variables);
}
