using System;
using System.Linq;
using Game.Level.Data;
using UnityEngine;

namespace Game.Level
{
    /// <summary>
    /// Wrapper for CollectablePoolFactories,
    /// each CollectableView has its own pool-factory by CollectableType 
    /// </summary>
    [Serializable]
    public class CollectablesFactory : IDisposable
    {
        [SerializeField] private CollectablePoolFactory[] _poolFactories;

        public CollectItemViewModel GetOrCreate(CollectableType type)
        {
            var factory = _poolFactories.FirstOrDefault(factory => factory.CollectableType == type);
            return factory?.GetOrCreate();
        }

        public void Release(CollectItemViewModel viewModel)
        {
            var factory =
                _poolFactories.FirstOrDefault(factory => factory.CollectableType == viewModel.CollectableType);
            factory?.Release(viewModel);
        }

        public void Dispose()
        {
            foreach (var poolFactory in _poolFactories)
            {
                poolFactory.Dispose();
            }
        }
    }
}