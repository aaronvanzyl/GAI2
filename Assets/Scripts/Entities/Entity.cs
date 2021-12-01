using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : IReadOnlyEntity
{
    public bool canBuyFrom { get; set; }
    public string name { get; set; }
    public bool canSellTo { get; set; }
    public int ID { get; set; }
    public Vector2Int pos { get; set; }
    public Expression money { get; set; }
    public IReadOnlyExpression readOnlyMoney => money;

    protected Dictionary<int, Expression> inventory = new Dictionary<int, Expression>();

    public bool QuerySellPrice(IReadOnlyWorld world, int itemID, int sellerID, out int price)
    {
        if (!canSellTo)
        {
            price = 0;
            return false;
        }
        price = world.GetItem(itemID).value;
        return true;
    }

    public bool QueryBuyPrice(IReadOnlyWorld world, int itemID, int buyerID, out int price)
    {
        if (!canBuyFrom)
        {
            price = 0;
            return false;
        }
        price = world.GetItem(itemID).value;
        return true;
    }

    public void AddItem(int itemID, Expression amount)
    {
        if (inventory.ContainsKey(itemID))
        {
            inventory[itemID] += amount;
        }
        else
        {
            inventory[itemID] = amount;
        }
    }

    public void ReduceItem(int itemID, Expression amount)
    {
        if (inventory.ContainsKey(itemID))
        {
            inventory[itemID] -= amount;
        }
        else
        {
            inventory[itemID] = -amount;
        }
    }

    public void CopyFrom(IReadOnlyEntity other)
    {
        ID = other.ID;
        pos = other.pos;
        money = new Expression(other.readOnlyMoney);
        inventory = new Dictionary<int, Expression>();
        foreach (int itemID in other.ItemsInInventory())
        {
            inventory[itemID] = new Expression(other.GetItemCount(itemID));
        }
    }

    public virtual Entity Clone() {
        Entity e = new Entity();
        e.CopyFrom(this);
        return e;
    }

    public virtual List<Action> GenerateItemActions(int actorID, int itemID, Expression amount)
    {
        return new List<Action>();
    }

    public IReadOnlyExpression GetItemCount(int itemID)
    {
        if (inventory.TryGetValue(itemID, out var amount))
        {
            return amount;
        }
        return new Expression(0);
    }

    public IEnumerable<int> ItemsInInventory() {
        return inventory.Keys;
    }
}
