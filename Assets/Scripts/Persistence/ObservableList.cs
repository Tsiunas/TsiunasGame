using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObservedList<T> : List<T>
{
    public event Action<int> Changed = delegate { };
    public event Action<T, bool> OnUpdated ;
    public event Action OnEmpty = delegate { };


    public new void Add(T item)
    {
        base.Add(item);
        if (OnUpdated != null)
            OnUpdated(item, true);
    }
    public new void Remove(T item)
    {
        base.Remove(item);
        if (OnUpdated != null)
            OnUpdated(item, false);

        if (Count == 0)
            if (OnEmpty != null)
                OnEmpty();
    }
}
