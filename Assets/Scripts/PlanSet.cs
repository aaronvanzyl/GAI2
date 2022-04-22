using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanSet
{
    public IReadOnlyWorld underlying;
    public Condition cond;
    public int actorID;
    public List<Plan> plans;
    public List<Plan> validPlans;

    public PlanSet(IReadOnlyWorld underlying, Condition cond, int actorID)
    {
        this.underlying = underlying;
        this.cond = cond;
        this.actorID = actorID;
        this.plans = new List<Plan>();
        this.validPlans = new List<Plan>();
    }
}
