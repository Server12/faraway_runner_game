using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Game.Level.Views;
using Game.Game.Level.Views.Factory;
using Game.Level.Controller;
using Game.Level.Spawners;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Game.Level.Generators
{
    
    /// <summary>
    /// Controlling Obstacle generation in LevelController Platforms obstacle spawners,
    /// </summary>
    public class ObstaclesGenerator :IInitializable, ITickable,IDisposable
    {
        private readonly ObstacleFactory _factory;
        private readonly ILevelController _levelController;
        private SpawnersModel obstacleSpawners;

        private readonly List<Obstacle> _generatedObstacles = new List<Obstacle>(50);

        public ObstaclesGenerator(ObstacleFactory factory, ILevelController levelController)
        {
            _factory = factory;
            _levelController = levelController;
        }

        public void Initialize()
        {
            _levelController.OnPlatformCreated += OnPlatformCreateHandler;
            _levelController.OnPlatformReleasedToPool += OnPlatformReleasedHandler;
        }

        private void OnPlatformReleasedHandler(PlatformModel platform)
        {
            var spawners = platform.obstacleSpawners.Spawners;
            for (int i = 0; i < spawners.Length; i++)
            {
                var spawner = spawners[i];
                spawner.isBusy = false;
                spawners[i] = spawner;
            }
        }

        private void OnPlatformCreateHandler(PlatformModel platform)
        {
            GenerateAsync(platform.obstacleSpawners).Forget();
        }

        private async UniTaskVoid GenerateAsync(SpawnersModel spawnersModel)
        {
            var spawners = spawnersModel.Spawners;
            var count = spawnersModel.MaxToGenerate;
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

                    var randomMaterial = Random.Range(0, _factory.TotalMaterials);
                    _generatedObstacles.Add(_factory.Spawn(randomMaterial,
                        spawnersModel.transform.position + spawner.localPos));
                    await UniTask.Yield(PlayerLoopTiming.LastUpdate);
                }

                count--;
            }
        }

        public void Tick()
        {
            for (int i = 0; i < _generatedObstacles.Count; i++)
            {
                if (!_generatedObstacles[i].isVisible)
                {
                    _factory.Release(_generatedObstacles[i]);
                    _generatedObstacles.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Dispose()
        {
            _levelController.OnPlatformCreated -= OnPlatformCreateHandler;
            _levelController.OnPlatformReleasedToPool -= OnPlatformReleasedHandler;
            _factory?.Dispose();
        }
    }
}