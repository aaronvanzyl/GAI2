using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OwnMoneyCondition : Condition
{
    public int ownerID;
    public Expression amount;

    public OwnMoneyCondition(int ownerID, Expression amount)
    {
        this.ownerID = ownerID;
        this.amount = amount;
    }

    public override List<Action> GenerateActions(IReadOnlyWorld world, int actorID, int maxActions)
    {
        if (actorID == ownerID) {
            List<Action> actions = new List<Action>();
            IReadOnlyEntity actor = world.GetReadOnlyEntity(actorID);
            List<int> possibleSaleItems = new List<int>();
            possibleSaleItems.AddRange(actor.ItemsInInventory());
            possibleSaleItems.AddRange(world.ItemsWithAttribute(ItemAttribute.SalePreference, 0));


            IEnumerable<IReadOnlyEntity> inRange = world.ReadOnlyEntitiesByDistance(actor.pos, actor.aiConfig.maxEntitySearchDist);
            foreach (IReadOnlyEntity merchant in inRange) {
                if (merchant.canSellTo) {
                    foreach (int itemID in possibleSaleItems) {
                        if (merchant.QuerySellPrice(world, itemID, actorID, out int price)) {
                            actions.Add(new SellAction(actorID, merchant.ID, itemID));
                        }
                    }
                }
            }
            return actions.Take(maxActions).ToList();
        }
        return new List<Action>();
    }

    public override bool SatisfiedSolved(IReadOnlyWorld world, IReadOnlyDictionary<int, int> variables)
    {
        IReadOnlyEntity owner = world.GetReadOnlyEntity(ownerID);
        return owner.readOnlyMoney.constant >= amount.Evaluate(variables);
    }

    //public override KVList<string, string> GetDisplayValues()
    //{
    //    KVList<string, string> displayValues = base.GetDisplayValues();
    //    displayValues.Add("Owner ID", ownerID.ToString());
    //    displayValues.Add("Amount", amount.ToString());
    //    return displayValues;
    //}

    public override void AddPropertiesTo(PropertyGroupRenderer renderer)
    {
        base.AddPropertiesTo(renderer);
        renderer.AddEntityIDProp("Owner", ownerID);
        renderer.AddExpressionProp("Amount", amount);
    }
    public override List<Inequality> GenerateInequalities(IReadOnlyWorld world)
    {
        IReadOnlyEntity owner = world.GetReadOnlyEntity(ownerID);
        Inequality moneyIneq = new Inequality(owner.readOnlyMoney, amount, Comparator.GEQ);
        return new List<Inequality>() { moneyIneq };
    }
}
