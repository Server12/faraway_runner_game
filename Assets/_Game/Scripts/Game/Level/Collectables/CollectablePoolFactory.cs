using System;
using Game.Core.FactoryPool;
using Game.Level.Data;
using UnityEngine;

namespace Game.Level
{
    /// <summary>
    /// CollectableViewMode pool factory by CollectableType
    /// </summary>
    [Serializable]
    public class CollectablePoolFactory : BaseFactoryPool<CollectItemViewModel>
    {
        [SerializeField] private CollectableType _collectableType;

        protected override void ActionOnGet(CollectItemViewModel poolObject)
        {
            poolObject.isVisible = true;
            base.ActionOnGet(poolObject);
        }
        

        public CollectableType CollectableType => _collectableType;
    }
}