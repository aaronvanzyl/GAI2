using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KVList<TKey, TValue>: IList<KeyValuePair<TKey, TValue>>
{
    List<KeyValuePair<TKey, TValue>> kvList = new List<KeyValuePair<TKey, TValue>>();

    public KeyValuePair<TKey, TValue> this[int index] { get => ((IList<KeyValuePair<TKey, TValue>>)kvList)[index]; set => ((IList<KeyValuePair<TKey, TValue>>)kvList)[index] = value; }

    public int Count => ((ICollection<KeyValuePair<TKey, TValue>>)kvList).Count;

    public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)kvList).IsReadOnly;

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)kvList).Add(item);
    }

    public void Add(TKey key, TValue value)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)kvList).Add(new KeyValuePair<TKey, TValue>(key, value));
    }

    public void Clear()
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)kvList).Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return ((ICollection<KeyValuePair<TKey, TValue>>)kvList).Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)kvList).CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<TKey, TValue>>)kvList).GetEnumerator();
    }

    public int IndexOf(KeyValuePair<TKey, TValue> item)
    {
        return ((IList<KeyValuePair<TKey, TValue>>)kvList).IndexOf(item);
    }

    public void Insert(int index, KeyValuePair<TKey, TValue> item)
    {
        ((IList<KeyValuePair<TKey, TValue>>)kvList).Insert(index, item);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return ((ICollection<KeyValuePair<TKey, TValue>>)kvList).Remove(item);
    }

    public void RemoveAt(int index)
    {
        ((IList<KeyValuePair<TKey, TValue>>)kvList).RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)kvList).GetEnumerator();
    }
}
