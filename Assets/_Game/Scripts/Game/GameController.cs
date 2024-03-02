using System;
using Cysharp.Threading.Tasks;
using Game.CameraController;
using Game.Controllers;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Game
{
    /// <summary>
    /// /Main GameController for controlling all game flow,Start,Restart,UI logic
    /// </summary>
    public class GameController : IStartable
    {
        private readonly ICameraController _cameraController;
        private readonly IPlayerController _playerController;

        public GameController(ICameraController cameraController, IPlayerController playerController)
        {
            _cameraController = cameraController;
            _playerController = playerController;
        }

        public void Start()
        {
            _playerController.OnDied += OnPlayerDiedHandler;
            _cameraController.SetTarget(_playerController.Transform);
        }

        private void OnPlayerDiedHandler()
        {
            _playerController.OnDied -= OnPlayerDiedHandler;
            ReloadSceneAsync().Forget();
        }

        private async UniTaskVoid ReloadSceneAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3));
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}