using Cinemachine;
using UnityEngine;

namespace Game.CameraController
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraViewModel : MonoBehaviour
    {
        public Camera Camera;
        public CinemachineVirtualCamera VirtualCamera;
    }
}