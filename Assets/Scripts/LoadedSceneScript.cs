using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace CameraStackTest
{
    public class LoadedSceneScript : MonoBehaviour
    {
        [SerializeField] private Camera overlayCamera;
        private IBaseCameraStack _baseCameraStack;
        private ISceneUnloader _sceneUnloader;

        public void Init(IBaseCameraStack baseCameraStack, ISceneUnloader sceneUnloader)
        {
            _baseCameraStack = baseCameraStack;
            _sceneUnloader = sceneUnloader;
            OnSceneLoad();
        }

        private void OnSceneLoad()
        {
            ConvertSceneCameraToOverlay(overlayCamera);
            _baseCameraStack.AddCameraToStack(overlayCamera);
        }

        private void ConvertSceneCameraToOverlay(Camera camera)
        {
            var cameraData = camera.GetUniversalAdditionalCameraData();
            if (cameraData.renderType != CameraRenderType.Overlay)
            {
                cameraData.renderType = CameraRenderType.Overlay;
            }
        }

        public void UnloadScene()
        {
            OnSceneUnload();
            _sceneUnloader.Unload(SceneManager.GetActiveScene());
        }

        private void OnSceneUnload()
        {
            _baseCameraStack.RemoveCameraFromStack(overlayCamera);
        }
    }
}
