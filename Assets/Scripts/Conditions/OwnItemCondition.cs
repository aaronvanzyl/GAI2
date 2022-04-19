using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class OwnItemCondition : Condition
{
    public int ownerID;
    public int itemID;
    public Expression amount;

    public OwnItemCondition(int ownerID, int itemID, Expression amount)
    {
        this.ownerID = ownerID;
        this.itemID = itemID;
        this.amount = amount;
    }

    public override List<Action> GenerateActions(IReadOnlyWorld world, int actorID, int max)
    {
        if (actorID == ownerID)
        {
            List<Action> actions = new List<Action>();
            IReadOnlyEntity actor = world.GetReadOnlyEntity(actorID);
            IEnumerable<IReadOnlyEntity> inRange = world.ReadOnlyEntitiesByDistance(actor.pos, actor.aiConfig.maxEntitySearchDist);
            foreach (IReadOnlyEntity other in inRange) {
                IReadOnlyList<Action> itemActions = other.GenerateItemActions(actorID, itemID);
                actions.AddRange(itemActions);
                if (other.QueryBuyPrice(world, itemID, actorID, out int price)) {
                    actions.Add(new BuyAction(actorID, other.ID, itemID));
                }
            }
            return actions.Take(max).ToList();
        }
        return new List<Action>();
    }

    public override bool SatisfiedSolved(IReadOnlyWorld world, Dictionary<int, int> variables)
    {
        IReadOnlyEntity owner = world.GetReadOnlyEntity(ownerID);
        return owner.GetItemCount(itemID).constant >= amount.Evaluate(variables);
    }

    //public override KVList<string, string> GetDisplayValues()
    //{
    //    KVList<string, string> displayValues = base.GetDisplayValues();
    //    displayValues.Add("Owner ID", ownerID.ToString());
    //    displayValues.Add("Item ID", itemID.ToString());
    //    displayValues.Add("Amount", amount.ToString());
    //    return displayValues;
    //}

    public override void AddPropertiesTo(PropertyGroupRenderer renderer)
    {
        base.AddPropertiesTo(renderer);
        renderer.AddEntityIDProp("Owner", ownerID);
        renderer.AddItemIDProp("Item", itemID);
        renderer.AddExpressionProp("Amount", amount);
    }

    public override List<Inequality> GenerateInequalities(IReadOnlyWorld world)
    {
        IReadOnlyEntity owner = world.GetReadOnlyEntity(ownerID);
        Inequality itemIneq = new Inequality(owner.GetItemCount(itemID), amount, Comparator.GEQ);
        return new List<Inequality>() { itemIneq };
    }
}
