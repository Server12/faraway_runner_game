using Game;
using Game.CameraController;
using Game.Controllers;
using Game.Data;
using Game.Game.Level.Views;
using Game.Level.Controller;
using Game.Level.Data;
using Game.Level.Generators;
using Game.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Faraway.Scopes
{
    /// <summary>
    /// Main Level Scope to registers dependencies to DI Container
    /// </summary>
    public class LevelScope : LifetimeScope
    {
        [Tooltip("On/Off generate obstacles/collectables")]
        [SerializeField] private bool _disableGeneration;
        [SerializeField] private GamePoolFactories _factories;
        [SerializeField] private CollectableEffectsConfig _collectableEffectsConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            if (!_disableGeneration)
            {
                builder.RegisterInstance(_factories.ObstaclesFactory);
                builder.RegisterInstance(_factories.CollectablesFactory);
                
                foreach (var effectAction in _collectableEffectsConfig.EffectActions)
                {
                    builder.RegisterInstance(effectAction).As<ICollectableEffectAction>();
                }
            
                builder.RegisterBuildCallback(resolver =>
                {
                    foreach (var effectAction in _collectableEffectsConfig.EffectActions)
                    {
                        effectAction.Initialize(resolver);
                    }
                });
                
                 builder.RegisterEntryPoint<ObstaclesGenerator>();
                 builder.RegisterEntryPoint<CollectablesGenerator>();
            }

            builder.RegisterComponentInHierarchy<PlayerViewModel>();
            builder.RegisterComponentInHierarchy<CameraViewModel>();
            builder.RegisterComponentInHierarchy<LevelViewModel>();

            builder.Register<PlayerInputController>(Lifetime.Singleton);
            builder.RegisterEntryPoint<PlayerController>().As<IPlayerController>();

            builder.RegisterEntryPoint<CameraController>().As<ICameraController>();

            builder.RegisterEntryPoint<LevelController>().As<ILevelController>();

            builder.RegisterEntryPoint<GameController>();
        }
    }
}