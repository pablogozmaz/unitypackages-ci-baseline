using System;
using System.Collections.Generic;
using UnityEngine;


namespace PixelsHub
{
    /// <summary>
    /// Generic implementation of a list of objects with object pooling.
    /// Pooled objects are deactivated via parenting changes.
    /// </summary>
    public abstract class ListPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        public event Action<T> OnInstantiatedItem;

        public int Count => items.Count;

        public IEnumerable<T> Items => items;

        protected abstract T ListItemPrefab { get; }

        protected readonly List<T> items = new();
        protected readonly Stack<T> pool = new();

        private Transform poolParent;


        public T this[int index] => items[index];

        protected virtual void Awake() 
        {
            if(ListItemPrefab != null)
                ListItemPrefab.gameObject.SetActive(false);

            poolParent = new GameObject("Pool Parent").transform;
            poolParent.transform.SetParent(transform);
            poolParent.gameObject.SetActive(false);
        }

        protected void PoolItems(int count)
        {
            for(int i = items.Count - 1; i >= count; i--)
            {
                PoolItem(i);
            }
        }

        protected void PoolItem(int index)
        {
            var item = items[index];
            pool.Push(item);
            item.transform.SetParent(poolParent);
            items.RemoveAt(index);
        }

        protected T GetItem(int index)
        {
            if(index < items.Count)
                return items[index];

            T item;

            if(pool.Count > 0)
            {
                item = pool.Pop();
                item.transform.SetParent(ListItemPrefab.transform.parent);
            }
            else
            {
                item = Instantiate(ListItemPrefab, ListItemPrefab.transform.parent);
                item.gameObject.SetActive(true);
                HandleInstantiatedItem(item);
                OnInstantiatedItem?.Invoke(item);
            }

            items.Add(item);

            return item;
        }

        protected virtual void HandleInstantiatedItem(T item) { }
    }
}
