using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KongC
{
    public class PreviewSceneStandalone : IDisposable
    {
        private readonly Scene previewScene;
        private Camera previewCamera;
        private List<Light> _lights = new List<Light>();
        private List<GameObject> _gameObjects = new List<GameObject>();
        private GameObject _root;
        private GameObject _rootLight;
        private GameObject _lookPoint;

        public List<Light> lights
        {
            get => _lights;
            set => _lights = value;
        }

        protected static Light CreateLight()
        {
            GameObject objectWithHideFlags = new GameObject("Light");
            Light component = objectWithHideFlags.AddComponent<Light>();
            component.cullingMask = 1 << LayerMask.NameToLayer("RenderObject");
            component.shadows = LightShadows.Soft;
            component.type = LightType.Directional;
            component.intensity = 1f;
            component.enabled = false;
            return component;
        }

        public void AddLight()
        {
            Light light = CreateLight();
            SetLayerRecursively(light.gameObject, LayerMask.NameToLayer("RenderObject"));
            light.transform.SetParent(_rootLight.transform);
            _lights.Add(light);
            Debug.LogWarning($"Light Spawn At : {light.transform.position}");
        }

        public Camera camera
        {
            get => previewCamera;
        }

        public GameObject AddGameObject(GameObject ObjectGo, Vector3 position)
        {
            GameObject spawn = GameObject.Instantiate(ObjectGo);
            SetLayerRecursively(spawn, LayerMask.NameToLayer("RenderObject"));
            spawn.transform.SetParent(_root.transform);
            spawn.transform.position = position;
            _gameObjects.Add(spawn);
            Debug.LogWarning($"Spawn At : {spawn.transform.position}");

            return spawn;
        }
        
        public void SetLayerRecursively(GameObject obj, LayerMask layerMask)
        {
            if (obj == null) return;

            obj.layer = layerMask;

            foreach (Transform child in obj.transform)
            {
                SetLayerRecursively(child.gameObject, layerMask);
            }
        }

        public void SetCameraLookAtPosition(Vector3 position)
        {
            _lookPoint.transform.position = position;
        }

        public PreviewSceneStandalone(Color backgroundColor, float cameraDistance = 10f)
        {
            // Create the scene
            previewScene = SceneManager.CreateScene("Preview Scene", new CreateSceneParameters(LocalPhysicsMode.None));
            
            // Create and setup the camera
            previewCamera = new GameObject("Preview Camera").AddComponent<Camera>();
            previewCamera.backgroundColor = backgroundColor;
            previewCamera.clearFlags = CameraClearFlags.SolidColor;
            previewCamera.transform.position = new Vector3(0, 0, -cameraDistance);
            previewCamera.transform.rotation = Quaternion.identity;
            previewCamera.cullingMask = 1 << LayerMask.NameToLayer("RenderObject");
            previewCamera.enabled = false;

            // Move the camera to the preview scene
            foreach (var light in _lights)
            {
                SceneManager.MoveGameObjectToScene(light.gameObject, previewScene);
            }

            _lookPoint = new GameObject("LookPoint");
            _lookPoint.layer = LayerMask.NameToLayer("RenderObject");
            _root = new GameObject("Root");
            _lookPoint.transform.SetParent(_root.transform);
            _root.layer = LayerMask.NameToLayer("RenderObject");
            SceneManager.MoveGameObjectToScene(_root, previewScene);
            _rootLight = new GameObject("Root Light");
            _rootLight.layer = LayerMask.NameToLayer("RenderObject");
            SceneManager.MoveGameObjectToScene(_rootLight, previewScene);
            SceneManager.MoveGameObjectToScene(previewCamera.gameObject, previewScene);
        }

        public void Render(RenderTexture renderTexture)
        {
            if (_root == null )
                throw new ArgumentNullException(nameof(_root));

            foreach (var light in _lights)
            {
                light.enabled = true;
            }
            
            
            // Position the camera to look at the target
            previewCamera.transform.LookAt(_lookPoint.transform);

            // Setup the render texture
            previewCamera.targetTexture = renderTexture;

            // Render the scene
            previewCamera.Render();

            // Reset the target texture to avoid issues
            previewCamera.targetTexture = null;

            // Optionally, move the object back to the original scene (if needed)
        }

        public void Dispose()
        {
            // Cleanup
            SceneManager.UnloadSceneAsync(previewScene);
            UnityEngine.Object.Destroy(previewCamera.gameObject);
        }
    }
}