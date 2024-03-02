using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Game.Core.FactoryPool
{
    /// <summary>
    /// Base generic Factory Pool for Unity Objects(monobehaviors,prefabs)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class BaseFactoryPool<T> : IDisposable where T : Object, IPooledObject
    {
        [SerializeField] private int _poolCapacity = 10;

        [SerializeField] protected T _prefab;

        private readonly ObjectPool<T> _objectPool;

        protected BaseFactoryPool()
        {
            _objectPool = new ObjectPool<T>(CreateFunc, ActionOnGet, ActionOnRelease, null, true, _poolCapacity);
        }

        protected virtual void ActionOnRelease(T poolObject)
        {
            poolObject.gameObject.SetActive(false);
        }

        protected virtual void ActionOnGet(T poolObject)
        {
            poolObject.gameObject.SetActive(true);
        }

        private T CreateFunc()
        {
            return Object.Instantiate(_prefab);
        }

        public void Release(T obj)
        {
            _objectPool.Release(obj);
        }

        public T GetOrCreate()
        {
            return _objectPool.Get();
        }

        public void Dispose()
        {
            _objectPool?.Dispose();
        }
    }
}