using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReadOnlyEntity
{
    public int ID { get; }
    public string name { get; }
    public Vector2Int pos { get; }
    public IReadOnlyExpression readOnlyMoney { get; }

    public List<Action> GenerateItemActions(int actorID, int itemID);
    public IReadOnlyExpression GetItemCount(int itemID);
    public IEnumerable<int> ItemsInInventory();
    //public IReadOnlyDictionary<int, IReadOnlyExpression> ReadOnlyInventory();

    public bool canBuyFrom { get; }
    public bool canSellTo { get; }
    /// <summary>
    /// Returns true if you can sell specified item to this entity.
    /// Writes sell value into price
    /// </summary>
    public bool QuerySellPrice(IReadOnlyWorld world, int itemID, int sellerID, out int price);
    public bool QueryBuyPrice(IReadOnlyWorld world, int itemID, int buyerID, out int price);

    public Entity Clone();
}
