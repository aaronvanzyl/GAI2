using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IReadOnlyWorld 
{
    public int maxEntityID { get; }
    public int maxItemID { get; }
    public IReadOnlyEntity GetReadOnlyEntity(int ID);
    public IEnumerable<IReadOnlyEntity> GetAllReadOnlyEntities();
    public IOrderedEnumerable<IReadOnlyEntity> ReadOnlyEntitiesByDistance(Vector2Int pos, float maxDist);
    public IEnumerable<int> AllEntityIDs();
    public IReadOnlyItem GetItem(int itemID);
    public IReadOnlyList<int> ItemsWithAttribute(ItemAttribute attribute, float minValue); 
}
