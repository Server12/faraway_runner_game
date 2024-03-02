using System;
using Game.CameraController;
using Game.Data;
using Game.Extensions;
using Game.Player;
using Game.Player.Data;
using UnityEngine;
using VContainer.Unity;

namespace Game.Controllers
{
    /// <summary>
    /// Main PlayerController
    /// Player Can:
    /// * AutoRun
    /// * Fly
    /// * Die
    /// All settings in PlayerConfig Scriptable
    /// </summary>
    public class PlayerController : IPlayerController, IInitializable, ITickable, IDisposable
    {
        public event Action OnDied;

        private static readonly int DieAnimatorProperty = Animator.StringToHash("die");
        private static readonly int RunAnimatorProperty = Animator.StringToHash("run");
        private static readonly int FlyAnimatorProperty = Animator.StringToHash("fly");

        private readonly PlayerViewModel _viewModel;
        private readonly PlayerInputController _playerInputController;
        private readonly PlayerConfig _config;
        private readonly ICameraController _cameraController;

        private PlayerState _prevState;
        private PlayerState _currentState;
        private Vector3 _velocity;
        private float _gravityVelocity;

        private float _moveSpeed;


        public PlayerController(PlayerViewModel viewModel, PlayerInputController playerInputController)
        {
            _viewModel = viewModel;
            _playerInputController = playerInputController;
            _config = _viewModel.Config;
        }

        public Transform Transform => _viewModel.CachedTransform;

        public PlayerState CurrentState
        {
            get => _currentState;
            private set
            {
                if (_currentState == value || _currentState == PlayerState.Die) return;
                _prevState = _currentState;
                _currentState = value;
            }
        }

        public float MoveSpeed
        {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }

        public Bounds Bounds => _viewModel.CharacterController.bounds;

        public PlayerConfig Config => _config;

        public void SetFlyMode(bool enable)
        {
            CurrentState = enable ? PlayerState.StartFly : PlayerState.AutoRun;
        }


        public void Initialize()
        {
            _playerInputController.Enable();
            MoveSpeed = Config.PlayerSpeed;
            _viewModel.OnHit += OnHitHandler;
            CurrentState = PlayerState.AutoRun;
        }

        public void Dispose()
        {
            _viewModel.OnHit -= OnHitHandler;
        }

        private void OnHitHandler(ControllerColliderHit hitCollider)
        {
            if (hitCollider.collider.CheckTags(GameTags.PlayerKillTags))
            {
                CurrentState = PlayerState.StartDie;
            }
        }


        public void Tick()
        {
            if (_currentState == PlayerState.Empty) return;
            switch (_currentState)
            {
                case PlayerState.AutoRun:

                    _viewModel.CachedTransform.rotation = Quaternion.identity;
                    _viewModel.Animator.SetBool(RunAnimatorProperty, true);
                    _velocity = new Vector3(
                        _playerInputController.swipeDirection * Time.deltaTime * _config.SwipeSpeed, _gravityVelocity,
                        _moveSpeed * Time.deltaTime);
                    UpdateInAir();
                    break;
                case PlayerState.StartFly:

                    _viewModel.Animator.SetBool(FlyAnimatorProperty, true);

                    _viewModel.CachedTransform.rotation = Quaternion.Slerp(_viewModel.CachedTransform.rotation,
                        Quaternion.Euler(_config.FlyTargetRotation, 0, 0), Time.deltaTime * _moveSpeed);

                    if (_viewModel.CachedTransform.position.y < _config.FlyHeight)
                    {
                        _gravityVelocity = Time.deltaTime * _config.FlySpeed;
                    }
                    else
                    {
                        _gravityVelocity = 0;
                    }

                    _velocity = new Vector3(_playerInputController.swipeDirection * Time.deltaTime * _config.SwipeSpeed,
                        _gravityVelocity, Time.deltaTime * _moveSpeed);

                    break;
                case PlayerState.StartDie:
                    _viewModel.CachedTransform.rotation = Quaternion.identity;
                    _playerInputController.Disable();
                    _velocity = Vector3.zero;
                    OnDied?.Invoke();
                    CurrentState = PlayerState.Die;
                    break;
                case PlayerState.Die:
                    _viewModel.Animator.SetTrigger(DieAnimatorProperty);
                    UpdateInAir();
                    CurrentState = _viewModel.CharacterController.isGrounded ? PlayerState.Empty : PlayerState.Die;
                    break;
            }

            _viewModel.CharacterController.Move(_velocity);

#if UNITY_EDITOR

            _viewModel.swipeInput = _playerInputController.swipeDirection;
            _viewModel.currentSpeed = _moveSpeed;
            _viewModel.currentState = _currentState;

#endif
        }


        private void UpdateInAir(bool applyVelocity = true)
        {
            if (applyVelocity)
            {
                if (_gravityVelocity > -_config.Gravity)
                    _gravityVelocity -= _config.Gravity * Time.smoothDeltaTime;
            }
            else
            {
                _gravityVelocity = 0f;
            }

            _viewModel.Animator.SetBool(FlyAnimatorProperty, !_viewModel.CharacterController.isGrounded);
        }
    }
}