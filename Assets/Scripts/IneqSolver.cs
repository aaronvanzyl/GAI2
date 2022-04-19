using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.SolverFoundation.Solvers;

public static class IneqSolver
{
    public static bool Solve(IEnumerable<Inequality> ineqs, out Dictionary<int, int> varDict)
    {
        // keys are expression var keys
        SortedSet<int> varKeys = new SortedSet<int>();
        foreach (Inequality ineq in ineqs)
        {
            varKeys.UnionWith(ineq.a.readonlyCoefficients.Keys);
            varKeys.UnionWith(ineq.b.readonlyCoefficients.Keys);
        }

        //int numVars = varKeys.Count;
        SimplexSolver solver = new SimplexSolver();
        List<int> varIDs = new List<int>();
        SortedList<int, int> keyToID = new SortedList<int, int>(); // Maps from expression-varKey to solver-varID


        foreach (int varKey in varKeys)
        {
            if (solver.AddVariable(varKey, out int varID))
            {
                solver.SetIntegrality(varID, true);
                solver.SetLowerBound(varID, 0);
                varIDs.Add(varID);
                keyToID[varKey] = varID;
            }
        }

        //foreach (var kvPair in keyToID) {
        //    Debug.Log($"{kvPair.Key}: {kvPair.Value}");
        //}

        foreach (Inequality ineq in ineqs)
        {
            solver.AddRow("Ineq " + solver.RowCount, out int rowID);
            Expression diff = new Expression(ineq.a) - new Expression(ineq.b);
            foreach (var kvPair in diff.coefficients)
            {
                solver.SetCoefficient(rowID, keyToID[kvPair.Key], kvPair.Value);
            }

            if (ineq.comparator == Comparator.GEQ)
            {
                solver.SetLowerBound(rowID, -diff.constant);
            }
            else if (ineq.comparator == Comparator.LEQ) {
                solver.SetUpperBound(rowID, -diff.constant);
            }
            else if (ineq.comparator == Comparator.EQ) {
                solver.SetBounds(rowID, -diff.constant, -diff.constant);
            }
        }
        solver.AddRow("Goal", out int goalID);
        foreach (int varID in varIDs) {
            solver.SetCoefficient(goalID, varID, 1);
        }
        solver.AddGoal(goalID, 1, true);

        var result = solver.Solve(new SimplexSolverParams());
        varDict = new Dictionary<int, int>();
        if (result.Result == Microsoft.SolverFoundation.Services.LinearResult.Feasible || result.Result == Microsoft.SolverFoundation.Services.LinearResult.Optimal) {
            foreach (int varKey in varKeys)
            {
                varDict[varKey] = (int)result.GetValue(keyToID[varKey]);
                //Debug.Log($"<{varKey}> = {result.GetValue(keyToID[varKey])}");
            }
            return true;
        }
        return false;
        //Debug.Log("Solver final result: " + result.Result);
        
    }


}
