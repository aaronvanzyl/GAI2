using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class World : IReadOnlyWorld
{
    IReadOnlyWorld underlying;
    Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
    Dictionary<int, IReadOnlyItem> itemDict = new Dictionary<int, IReadOnlyItem>();
    Dictionary<ItemTag, List<int>> itemsWithTagDict = new Dictionary<ItemTag, List<int>>();

    public int maxItemID { get; private set; }
    public int maxEntityID { get; private set; }

    public World()
    {
        underlying = null;
        maxEntityID = -1;
        maxItemID = -1;
    }

    public World(IReadOnlyWorld underlying)
    {
        this.underlying = underlying;
        maxEntityID = underlying.maxEntityID;
        maxItemID = underlying.maxItemID;
    }

    public void AddEntity(Entity entity)
    {
        maxEntityID++;
        entity.ID = maxEntityID;
        entities.Add(entity.ID, entity);
    }

    public Entity GetEntity(int ID)
    {
        if (entities.TryGetValue(ID, out Entity entity))
        {
            return entity;
        }
        Entity clone = underlying.GetReadOnlyEntity(ID).Clone();
        entities.Add(ID, clone);
        return clone;
    }

    public IReadOnlyEntity GetReadOnlyEntity(int ID)
    {
        if (entities.TryGetValue(ID, out Entity entity))
        {
            return entity;
        }
        return underlying.GetReadOnlyEntity(ID);
    }

    public IEnumerable<IReadOnlyEntity> GetAllReadOnlyEntities()
    {
        List<IReadOnlyEntity> entities = new List<IReadOnlyEntity>();
        foreach (int ID in AllEntityIDs())
        {
            entities.Add(GetReadOnlyEntity(ID));
        }
        return entities;
    }

    public IEnumerable<int> AllEntityIDs()
    {
        HashSet<int> IDs = new HashSet<int>();
        IDs.UnionWith(entities.Keys);
        if (underlying != null)
        {
            IDs.UnionWith(underlying.AllEntityIDs());
        }
        return IDs.ToList();
    }

    public IOrderedEnumerable<IReadOnlyEntity> ReadOnlyEntitiesByDistance(Vector2Int pos, float maxDist)
    {
        IEnumerable<IReadOnlyEntity> filtered = GetAllReadOnlyEntities().Where(x => Distance(x.pos, pos) <= maxDist);
        return filtered.OrderBy(x => Distance(pos, x.pos));
    }

    public static float Distance(Vector2Int pos1, Vector2Int pos2)
    {
        return Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y);
    }

    public int RegisterItem(IReadOnlyItem item)
    {
        maxItemID++;
        itemDict.Add(maxItemID, item);
        foreach (ItemTag tag in item.readOnlyTags)
        {
            if (itemsWithTagDict.TryGetValue(tag, out List<int> taggedItems))
            {
                taggedItems.Add(maxItemID);
            }
            else
            {
                itemsWithTagDict[tag] = new List<int> { maxItemID };
            }

        }
        return maxItemID;
    }

    public IReadOnlyList<int> ItemsWithTag(ItemTag tag)
    {
        if (itemsWithTagDict.TryGetValue(tag, out List<int> items))
        {
            return items;
        }
        return new List<int>();
    }

    public IReadOnlyItem GetItem(int itemID)
    {
        if (itemDict.TryGetValue(itemID, out IReadOnlyItem item))
        {
            return item;
        }
        return underlying.GetItem(itemID);
    }

}
