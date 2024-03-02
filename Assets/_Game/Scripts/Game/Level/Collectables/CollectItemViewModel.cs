using System;
using Game.Core.FactoryPool;
using Game.Data;
using Game.Level.Data;
using UnityEngine;

namespace Game.Level
{
    /// <summary>
    /// CollectableViewModel, detect player trigger collisions,
    /// send collect events
    /// </summary>
    public class CollectItemViewModel : MonoBehaviour, IPooledObject
    {
        public event Action<CollectItemViewModel> OnPlayerCollected;

        [HideInInspector] public CollectableType CollectableType;
        public Collider Collider;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Transform _child;

        [HideInInspector] public bool isVisible = true;

        private void OnBecameInvisible()
        {
            isVisible = false;
        }

        private void OnBecameVisible()
        {
            isVisible = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(GameTags.PlayerTag))
            {
                OnPlayerCollected?.Invoke(this);
            }
        }

        public void StartAnimate()
        {
        }

        public void StopAnimation()
        {
        }
    }
}