using System;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = Game.Player.Input.PlayerInput;

namespace Game.Controllers
{
    /// <summary>
    /// Input controller wrapper for Player
    /// </summary>
    public class PlayerInputController:IDisposable
    {
        private readonly PlayerInput _playerInput;

        private Vector2 _swipeInput;
        public float swipeDirection;

        private readonly InputAction _touchAction;
        
        public PlayerInputController()
        {
            _playerInput = new PlayerInput();
            _touchAction = _playerInput.Player.Touch;
        }

        public void Enable()
        {
            _playerInput.Enable();
            _playerInput.Player.Swipe.performed += OnSwipePerformed;
            _playerInput.Player.Swipe.canceled += OnSwipeCanceled;

        }

        public void Disable()
        {
            swipeDirection = 0f;
            _playerInput.Disable();
            _playerInput.Player.Swipe.performed -= OnSwipePerformed;
            _playerInput.Player.Swipe.canceled -= OnSwipeCanceled;
        }

        private void OnSwipeCanceled(InputAction.CallbackContext obj)
        {
            swipeDirection = 0f;
            _swipeInput = Vector2.zero;
        }


        private void OnSwipePerformed(InputAction.CallbackContext ctx)
        {
            if (!_touchAction.IsPressed())
            {
                swipeDirection = 0f;
                _swipeInput = Vector2.zero;
                return;
            }
            _swipeInput = ctx.ReadValue<Vector2>();
            swipeDirection = _swipeInput.x;
        }

        public void Dispose()
        {
            _playerInput.Dispose();
        }
    }
}