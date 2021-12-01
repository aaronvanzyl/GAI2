using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Comparator { LEQ, EQ, GEQ };

public class Inequality
{
    static string[] comparatorStr = new string[] { "<=", "=", ">="};

    public IReadOnlyExpression a;
    public IReadOnlyExpression b;
    public Comparator comparator;

    public Inequality(IReadOnlyExpression a, IReadOnlyExpression b, Comparator comparator)
    {
        this.a = a.Clone();
        this.b = b.Clone();
        this.comparator = comparator;
    }

    public override string ToString() {
        return a + " " + comparatorStr[(int)comparator] + " " + b;
    }
}
