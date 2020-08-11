using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// Use for pooling simple Component object
    /// </summary>
    /// <typeparam name="T">component</typeparam>
    public sealed class SimplePool<T> : IPool<T> where T : Component
    {
        private int _poolGrowth;
        private Transform _poolParent;

        private List<T> _inactiveObjects = new List<T>();
        private List<T> _activeObjects = new List<T>();

        private T basePoolObject;

        /// <summary>
        /// Creating the simple pool.
        /// <para> </para>
        /// Base methods: GetOnPool, SetToPool, ClearPool.
        /// </summary>
        /// <param name="objectForPool">object for pooling</param>
        /// <param name="poolParent">parent for all object in the pool</param>
        /// <param name="startPoolSize">start pool size</param>
        /// <param name="poolGrowth">count of new objects creating while the pool is empty (increasing the pool size)</param>
        public SimplePool(T objectForPool, Transform poolParent, int startPoolSize, int poolGrowth = 2)
        {
            basePoolObject = objectForPool;

            _poolGrowth = poolGrowth;
            _poolParent = poolParent;

            objectForPool.gameObject.SetActive(false);
            objectForPool.transform.SetParent(_poolParent);
            _inactiveObjects.Add(objectForPool);

            for (int i = 0; i < startPoolSize; i++)
            {
                _inactiveObjects.Add(CreateObjectInPool());
            }
        }

        /// <summary>
        /// Get object from pool
        /// </summary>
        /// <returns>object from pool</returns>
        public T GetOnPool()
        {
            if (_inactiveObjects.Count == 0)
            {
                for (int i = 0; i < _poolGrowth; i++)
                {
                    _inactiveObjects.Add(CreateObjectInPool());
                }
            }

            var newObject = _inactiveObjects[0];
            _inactiveObjects.RemoveAt(0);
            _activeObjects.Add(newObject);
            return newObject;
        }

        /// <summary>
        /// Return object in pool
        /// </summary>
        /// <param name="go">object for return to pool</param>
        public void SetToPool(T go)
        {
            _activeObjects.Remove(go);
            go.gameObject.SetActive(false);
            go.transform.SetParent(_poolParent);
            _inactiveObjects.Add(go);
        }
        
        /// <summary>
        /// Clear pool with destroying all pool objects
        /// </summary>
        public void ClearPool()
        {
            _inactiveObjects.ForEach(Object.Destroy);
            _inactiveObjects = new List<T>();
            _activeObjects.ForEach(Object.Destroy);
            _activeObjects = new List<T>();
        }
        
        /// <summary>
        /// Create object in pool
        /// </summary>
        /// <returns>new object</returns>
        private T CreateObjectInPool()
        {
            T newObject = GameObject.Instantiate(basePoolObject, _poolParent);

            newObject.gameObject.SetActive(false);
            newObject.transform.SetParent(_poolParent);
            return newObject;
        }
    }
}