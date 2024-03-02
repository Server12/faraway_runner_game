using System;
using Game.Controllers;
using UnityEngine;
using VContainer;

namespace Game.Level.Data
{
    [CreateAssetMenu(fileName = "PlayerStartFlyEffectAction")]
    public class PlayerStartFlyEffectAction:BaseCollectableEffectAction
    {
        public override CollectableType CollectableType => CollectableType.FlyBoosterItem;
        
        private IPlayerController _playerController;

        public override void Initialize(IObjectResolver resolver)
        {
            _playerController = resolver.Resolve<IPlayerController>();
        }
        
        protected override void ActivateInternal()
        {
            _playerController.SetFlyMode(true);
        }

        protected override void DeactivateInternal()
        {
            _playerController.SetFlyMode(false);
        }
    }
}