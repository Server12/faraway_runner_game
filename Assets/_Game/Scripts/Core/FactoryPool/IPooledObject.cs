using UnityEngine;

namespace Game.Core.FactoryPool
{
    public interface IPooledObject
    {
        GameObject gameObject { get; }
    }
}