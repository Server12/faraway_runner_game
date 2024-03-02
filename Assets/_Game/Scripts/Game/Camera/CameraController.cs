using Cinemachine;
using UnityEngine;

namespace Game.CameraController
{
    /// <summary>
    /// Cameras Controller wrapper
    /// </summary>
    public class CameraController : ICameraController
    {
        private readonly CameraViewModel _cameraViewModel;

        public CameraController(CameraViewModel cameraViewModel)
        {
            _cameraViewModel = cameraViewModel;
        }

        public void SetTarget(Transform target)
        {
            _cameraViewModel.VirtualCamera.Follow = target;
            _cameraViewModel.VirtualCamera.LookAt = target;
        }
    }
}