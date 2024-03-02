using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Game.Level.Data
{
    
    /// <summary>
    /// Base EffectAction ScriptableObject
    /// Controlling Activate/Deactivate effect and activation timer
    /// </summary>
    public abstract class BaseCollectableEffectAction : ScriptableObject, ICollectableEffectAction
    {
        [SerializeField] private float _activationTime;

        private float _activationTimer;

        public abstract CollectableType CollectableType { get; }

        public float ActivationTime => _activationTime;

        protected abstract void ActivateInternal();

        protected abstract void DeactivateInternal();


        /// <summary>
        /// Call from LevelScope, after register
        /// </summary>
        /// <param name="resolver"></param>
        public virtual void Initialize(IObjectResolver resolver)
        {
        }

        public void Activate()
        {
            _activationTimer = _activationTime;
           // Debug.Log($"{GetType().Name} effect activated");
            ActivateInternal();
        }

        public void Deactivate()
        {
            _activationTimer = 0f;
            //Debug.Log($"{GetType().Name} effect deactivated");
            DeactivateInternal();
        }

        public void UpdateTimer(float deltaTime)
        {
            if (_activationTimer > 0)
            {
                _activationTimer -= deltaTime;
                if (_activationTimer <= 0)
                {
                    _activationTimer = 0f;
                    Deactivate();
                }
            }
        }
        
    }
}