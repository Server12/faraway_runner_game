using System;
using UnityEngine;

namespace Game.Player.Data
{
    /// <summary>
    /// Main PlayerController Configuration
    /// </summary>
    //[CreateAssetMenu(fileName = "PlayerConfig", menuName = "MENUNAME", order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private float playerSpeed = 5f;
        [Range(1f, 20)] [SerializeField] private float _gravity;

        [SerializeField] private float _flyHeight;
        [SerializeField] private float _flySpeed;

        [SerializeField] private float _flyTargetRotation = 50;
        [SerializeField] private float _swipeSpeed;
        
        public float PlayerSpeed => playerSpeed;

        public float FlySpeed => _flySpeed;

        public float FlyHeight => _flyHeight;

        public float Gravity => _gravity;

        public float SwipeSpeed => _swipeSpeed;

        public float FlyTargetRotation => _flyTargetRotation;
    }
}