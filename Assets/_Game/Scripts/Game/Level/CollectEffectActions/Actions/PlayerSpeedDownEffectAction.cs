using System;
using Game.Controllers;
using UnityEngine;
using VContainer;

namespace Game.Level.Data
{
    [CreateAssetMenu(fileName = "PlayerSpeedDownEffectAction")]
    public class PlayerSpeedDownEffectAction:BaseCollectableEffectAction
    {
        [Range(0.25f,1f)]
        [SerializeField] private float _speedMultiplier;

        public override CollectableType CollectableType => CollectableType.SpeedDownBoosterItem;

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