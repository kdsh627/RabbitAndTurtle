using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Manager
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance { get; private set; }

        [SerializeField] private Camera _uiCamera;

        private CinemachineCamera _camera;

        public Camera UICamera => _uiCamera;
        public CinemachineCamera CinemachineCamera
        {
            get { return _camera; }
            set { _camera = value; }
        }

        private void Awake()
        {
            Instance = this;
            SceneEventHandler.SceneStarted += AddStackCamera;
        }

        void OnDestroy()
        {
            SceneEventHandler.SceneStarted -= AddStackCamera;
        }

        private void AddStackCamera()
        {
            Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(_uiCamera);
        }
    }
}