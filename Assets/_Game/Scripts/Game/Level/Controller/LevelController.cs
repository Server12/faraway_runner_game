using System;
using System.Collections.Generic;
using Game.Data;
using Game.Game.Level.Views;
using UnityEngine;
using UnityEngine.Pool;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Game.Level.Controller
{
    
    /// <summary>
    /// Level platforms manager,
    /// generate endless level by snapping platform views, one after another,
    /// relative to PlayerController autorun
    /// </summary>
    public class LevelController : ILevelController, IInitializable, IStartable, ITickable
    {
        public event Action<PlatformModel> OnPlatformCreated;

        public event Action<PlatformModel> OnPlatformReleasedToPool;

        private readonly LevelViewModel _viewModel;

        private ObjectPool<PlatformModel> _pool;
        private PlatformModel _currentPlatform;

        private List<PlatformModel> _prevPlatforms;

        public LevelController(LevelViewModel viewModel)
        {
            _viewModel = viewModel;
            _prevPlatforms = new List<PlatformModel>();
        }

        public void Start()
        {
            _currentPlatform = _viewModel.platform;

            OnPlatformCreated?.Invoke(_currentPlatform);

            _currentPlatform.TriggerEvents.TriggerExit += OnTriggerExitHandler;
        }

        public void Initialize()
        {
            _pool = new ObjectPool<PlatformModel>(CreateFunc, ActionOnGet, ActionOnRelease);
        }

        public void Tick()
        {
            //release platforms which invisible by camera
            for (int i = 0; i < _prevPlatforms.Count; i++)
            {
                if (_prevPlatforms[i].gameObject.activeSelf && !_prevPlatforms[i].isVisible)
                {
                    _pool.Release(_prevPlatforms[i]);
                    _prevPlatforms.RemoveAt(i);
                    i--;
                }
            }
        }

        #region PLATFORMS GENERATION & POOL

        private void ActionOnRelease(PlatformModel nextPlatform)
        {
            OnPlatformReleasedToPool?.Invoke(nextPlatform);
            nextPlatform.gameObject.SetActive(false);
            nextPlatform.TriggerEvents.TriggerExit -= OnTriggerExitHandler;
        }

        private void ActionOnGet(PlatformModel nextPlatform)
        {
            nextPlatform.gameObject.SetActive(true);
            nextPlatform.TriggerEvents.TriggerExit -= OnTriggerExitHandler;
            nextPlatform.TriggerEvents.TriggerExit += OnTriggerExitHandler;
        }

        private void OnTriggerExitHandler(Collider collider)
        {
            if (collider.CompareTag(GameTags.PlayerTag))
            {
                _prevPlatforms.Add(_currentPlatform);
                var nextPos = _currentPlatform.nextSpawner.position;
                _currentPlatform = _pool.Get();
                _currentPlatform.transform.position = nextPos;
                OnPlatformCreated?.Invoke(_currentPlatform);
            }
        }

        private PlatformModel CreateFunc()
        {
            return Object.Instantiate(_viewModel.platform, _viewModel.transform);
        }

        #endregion
    }
}