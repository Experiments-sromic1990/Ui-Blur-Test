using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CameraStackTest
{
    public interface ISceneUnloader
    {
        void Unload(Scene scene);
    }
    
    public class BaseSceneScript : MonoBehaviour, IBaseCameraStack, ISceneUnloader
    {
        [SerializeField] private Camera baseCamera;
        [SerializeField] private GraphicRaycaster graphicRaycaster;
        [SerializeField] private GameObject blurContent;

        public void AddCamerasToStack(List<Camera> cameras)
        {
            for (int i = 0; i < cameras.Count; i++)
            {
                AddCameraToStack(cameras[i]);
            }
        }

        public void AddCameraToStack(Camera camera)
        {
            var cameraData = baseCamera.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(camera);
        }


        public void RemoveCamerasFromStack(List<Camera> cameras)
        {
            for (int i = 0; i < cameras.Count; i++)
            {
                RemoveCameraFromStack(cameras[i]);
            }
        }
        public void RemoveCameraFromStack(Camera camera)
        {
            var cameraData = baseCamera.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(camera);
        }

        public void LoadNextScene()
        {
            StartCoroutine(LoadChildScene("NextScene"));
        }

        private IEnumerator LoadChildScene(string sceneName)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            yield return operation;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            graphicRaycaster.enabled = false;
            blurContent.SetActive(true);
            
            
            Scene currentScene = SceneManager.GetActiveScene();
            var objs = currentScene.GetRootGameObjects();
            foreach (GameObject gObj in objs)
            {
                LoadedSceneScript script = gObj.GetComponent<LoadedSceneScript>();
                if (script != null)
                {
                    script.Init(this, this);
                    break;
                }
            }
        }

        public void Unload(Scene scene)
        {
            if (scene.isLoaded)
            {
                StartCoroutine(UnloadChildScene(scene.name));
            }
        }

        private IEnumerator UnloadChildScene(string sceneName)
        {
            AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            yield return operation;
            graphicRaycaster.enabled = true;
            blurContent.SetActive(false);
        }
    }
}

