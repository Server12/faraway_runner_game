using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Game.Level.Views;
using Game.Level.Controller;
using Game.Level.Data;
using Game.Level.Spawners;
using UnityEngine;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Game.Level.Generators
{
    
    /// <summary>
    /// Controlling collectables generation for LevelController platform,
    /// Detect player hit with Collectables, activate/deactivate Collectable effects
    /// </summary>
    public class CollectablesGenerator : IInitializable, IDisposable, ITickable
    {
        private readonly CollectablesFactory _collectablesFactory;
        private readonly ILevelController _levelController;
        private readonly IReadOnlyList<ICollectableEffectAction> _effectActions;

        private readonly List<CollectItemViewModel> _generatedCollectItems = new(50);


        private ICollectableEffectAction _prevActivatedEffect;

        public CollectablesGenerator(CollectablesFactory collectablesFactory, ILevelController levelController,
            IReadOnlyList<ICollectableEffectAction> effectActions)
        {
            _collectablesFactory = collectablesFactory;
            _levelController = levelController;
            _effectActions = effectActions;
        }

        private async UniTaskVoid GenerateAsync(SpawnersModel collectablesSpawners)
        {
            var spawners = collectablesSpawners.Spawners;
            var count = collectablesSpawners.MaxToGenerate;
            await UniTask.Yield(PlayerLoopTiming.LastUpdate);
            while (count > 0)
            {
                await UniTask.Yield(PlayerLoopTiming.LastUpdate);
                var index = Random.Range(0, spawners.Length);
                var spawner = spawners[index];
                if (!spawner.isBusy)
                {
                    spawner.isBusy = true;
                    spawners[index] = spawner;

                    var effectIndex = Random.Range(0, _effectActions.Count);

                    var collectableType = _effectActions[effectIndex].CollectableType;

                    var collectable = _collectablesFactory.GetOrCreate(collectableType);
                    collectable.CollectableType = collectableType;
                    collectable.OnPlayerCollected += OnPlayerCollectedHandler;
                    collectable.transform.position = collectablesSpawners.transform.position + spawner.localPos;

                    _generatedCollectItems.Add(collectable);

                    await UniTask.Yield(PlayerLoopTiming.LastUpdate);
                }

                count--;
            }
        }

        private void OnPlayerCollectedHandler(CollectItemViewModel collectItemViewModel)
        {
            collectItemViewModel.OnPlayerCollected -= OnPlayerCollectedHandler;
            _collectablesFactory.Release(collectItemViewModel);

            _prevActivatedEffect?.Deactivate();

            _prevActivatedEffect = _effectActions.FirstOrDefault(action =>
                action.CollectableType == collectItemViewModel.CollectableType);
            _prevActivatedEffect?.Activate();
        }


        public void Initialize()
        {
            _levelController.OnPlatformReleasedToPool += OnPlatformReleasedHandler;
            _levelController.OnPlatformCreated += OnPlatformCreatedHandler;
        }

        private void OnPlatformReleasedHandler(PlatformModel model)
        {
            var spawners = model.collectableSpawners.Spawners;
            for (int i = 0; i < spawners.Length; i++)
            {
                var spawner = spawners[i];
                spawner.isBusy = false;
                spawners[i] = spawner;
            }
        }

        private void OnPlatformCreatedHandler(PlatformModel model)
        {
            GenerateAsync(model.collectableSpawners).Forget();
        }

        public void Dispose()
        {
            _collectablesFactory.Dispose();
            _levelController.OnPlatformReleasedToPool -= OnPlatformReleasedHandler;
            _levelController.OnPlatformCreated -= OnPlatformCreatedHandler;
        }

        public void Tick()
        {
            _prevActivatedEffect?.UpdateTimer(Time.deltaTime);

            for (int i = 0; i < _generatedCollectItems.Count; i++)
            {
                if (!_generatedCollectItems[i].isVisible)
                {
                    _generatedCollectItems[i].OnPlayerCollected -= OnPlayerCollectedHandler;
                    _collectablesFactory.Release(_generatedCollectItems[i]);
                    _generatedCollectItems.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}