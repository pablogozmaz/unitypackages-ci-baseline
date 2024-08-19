using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelsHub
{
    [Serializable]
    public class GameObjectPool<T> where T : MonoBehaviour
    {
        private static Transform PooledObjectsParent
        {
            get 
            {
                if(pooledObjectsParent == null)
                {
                    pooledObjectsParent = new GameObject("Objects Pool Parent").transform;
                    pooledObjectsParent.gameObject.SetActive(false);
                    UnityEngine.Object.DontDestroyOnLoad(pooledObjectsParent);
                }

                return pooledObjectsParent;
            }
        }

        private static Transform pooledObjectsParent;

        private readonly Stack<T> pool = new();

        [SerializeField]
        private T prefab;

        public T Get(Transform parent)
        {
            T result;

            if(pool.Count > 0)
            {
                result = pool.Pop();
                result.transform.SetParent(parent);
            }
            else
            {
                result = UnityEngine.Object.Instantiate(prefab, parent);
                InitializeInstantiatedObject(result);
            }

            return result;
        }

        public void Pool(T obj)
        {
            pool.Push(obj);
            obj.transform.SetParent(PooledObjectsParent);
        }

        protected virtual void InitializeInstantiatedObject(T obj) { }
    }
}
