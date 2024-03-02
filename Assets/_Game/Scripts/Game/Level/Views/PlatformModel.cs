using System;
using Game.Helpers;
using Game.Level.Spawners;
using UnityEngine;

namespace Game.Game.Level.Views
{
    /// <summary>
    /// PlatformViewModel for LevelController,
    /// with indicator triggers, and spawners
    /// </summary>
    public class PlatformModel : MonoBehaviour
    {
        public TriggerEvents TriggerEvents;
        public Transform nextSpawner;
        [HideInInspector] public bool isVisible = true;
        public SpawnersModel obstacleSpawners;
        public SpawnersModel collectableSpawners;
        
        
        private void OnBecameVisible()
        {
            isVisible = true;
        }

        private void OnBecameInvisible()
        {
            isVisible = false;
        }
    }
}