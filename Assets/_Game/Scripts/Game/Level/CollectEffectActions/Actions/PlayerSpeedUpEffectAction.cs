using System;
using Game.Controllers;
using UnityEngine;
using VContainer;

namespace Game.Level.Data
{
    [CreateAssetMenu(fileName = "PlayerSpeedUpEffectAction")]
    public class PlayerSpeedUpEffectAction : BaseCollectableEffectAction
    {
        [Range(2f, 4f)] [SerializeField] private float _speedMultiplier;

        public override CollectableType CollectableType => CollectableType.SpeedUpBoosterItem;
        
        private IPlayerController _playerController;

        public override void Initialize(IObjectResolver resolver)
        {
            _playerController = resolver.Resolve<IPlayerController>();
        }
        
        protected override void ActivateInternal()
        {
            _playerController.MoveSpeed = _playerController.Config.PlayerSpeed * _speedMultiplier;
        }

        protected override void DeactivateInternal()
        {
            _playerController.MoveSpeed = _playerController.Config.PlayerSpeed;
        }
    }
}