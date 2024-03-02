using System;
using Game.Player.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Player
{
    /// <summary>
    /// Player ViewModel with collision event 
    /// </summary>
    public class PlayerViewModel : MonoBehaviour
    {
        public event Action<ControllerColliderHit> OnHit;

        public PlayerConfig Config;
        public Transform CachedTransform;
        public CharacterController CharacterController;
        public Animator Animator;

        #if UNITY_EDITOR

        [Header("DEBUG")] public float swipeInput;
        public float currentSpeed;
        public PlayerState currentState;
        
        #endif
        
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            OnHit?.Invoke(hit);
        }
    }
}