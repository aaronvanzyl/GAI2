using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plan
{
    public PathNode root;
    public bool valid;
    public List<Node> traverseOrder;
    public IReadOnlyDictionary<int, int> varDict;
    public IReadOnlyDictionary<int, IEnumerable<Inequality>> ineqs;
    public IReadOnlyDictionary<int, float> costs;
    public float totalCost;

    public Plan(PathNode root, bool valid, List<Node> traverseOrder, IReadOnlyDictionary<int, int> varDict, IReadOnlyDictionary<int, IEnumerable<Inequality>> ineqs, IReadOnlyDictionary<int, float> costs, float totalCost)
    {
        this.root = root;
        this.valid = valid;
        this.traverseOrder = traverseOrder;
        this.varDict = varDict;
        this.ineqs = ineqs;
        this.costs = costs;
        this.totalCost = totalCost;
    }
}
