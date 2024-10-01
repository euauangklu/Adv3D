using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KongC
{
    public class PreviewRoom
    {
        public Color cameraBackgroundColor = new Color(0,0,0,0);
        public Vector3 cameraPosition = new Vector3(0, 0, -1.5f);
        public RenderTexture renderTexture = new RenderTexture(1920, 1080, 32);
        public CameraClearFlags cameraClearFlags = CameraClearFlags.SolidColor;
        public bool orthographic = false;
        public float fieldOfView = 60f;
        public float nearClipPlane = 0.01f;
        public Vector3 lightRotation = Vector3.zero;
        public Vector3 lightPosition = new Vector3(0, 0, -5);
        public float lightRange = 100;
        public float lightIntensity = 5;
        private GameObject prefabInScene;
        private PreviewRenderUtility _previewRenderUtility;
        public PreviewRenderUtility PreviewRenderUtility
        {
            get => _previewRenderUtility;
        }

        public GameObject PrefabInScene
        {
            get => prefabInScene;
        }
        
        public void OnInitialization()
        {
            if(_previewRenderUtility == null)
                InitializeScene();
        }
        
        private void InitializeScene()
        {
            _previewRenderUtility = new PreviewRenderUtility();
            
            //Setting Camera
            Camera camera = _previewRenderUtility.camera;
            camera.backgroundColor = cameraBackgroundColor;
            camera.transform.position = cameraPosition;
            _previewRenderUtility.camera.orthographic = orthographic;
            _previewRenderUtility.camera.fieldOfView = fieldOfView;
            _previewRenderUtility.camera.nearClipPlane = nearClipPlane;
            camera.clearFlags = cameraClearFlags;
            
            //Setting Anti-aliasing
            renderTexture.antiAliasing = 1;
            camera.targetTexture = renderTexture;
            
            //Setting Light
            _previewRenderUtility.lights[0].transform.rotation = Quaternion.Euler(lightRotation);
            _previewRenderUtility.lights[0].transform.position = lightPosition;
            _previewRenderUtility.lights[0].range = lightRange;
            _previewRenderUtility.lights[0].intensity = lightIntensity;
        }

        public GameObject AddPrefabInScene(GameObject gameObject)
        {
            prefabInScene = PreviewRenderUtility.InstantiatePrefabInScene(gameObject);
            Debug.LogWarning($"Spawn At : {prefabInScene.transform.position}");

            return prefabInScene;
        }
        
        public GameObject AddGameObjectInScene(GameObject gameObject, Vector3 position)
        {
            GameObject objectInScene = GameObject.Instantiate(gameObject);
            objectInScene.transform.SetParent(prefabInScene.transform);
            objectInScene.transform.position = position;
            Debug.LogWarning($"Spawn At : {objectInScene.transform.position}");

            return objectInScene;
        }
        
        public GameObject AddGameObjectInScene(GameObject gameObject)
        {
            return AddGameObjectInScene(gameObject, gameObject.transform.position);
        }

        public void Destroy(Object unityObject)
        {
            //Begin Preview
            Rect renderRect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            _previewRenderUtility.BeginPreview(renderRect, GUIStyle.none);
            GameObject.Destroy(unityObject);
            _previewRenderUtility.camera.Render();
            PreviewRenderUtility.EndPreview();
        }

        public Texture CaptureScene(Vector2Int size, Color BGColor, out RenderTexture RTexture)
        {
            renderTexture = new RenderTexture(size.x, size.y, 32);
            cameraBackgroundColor = BGColor;
            
            //Begin Preview
            Rect renderRect = new Rect(0, 0, size.x, size.y);
            PreviewRenderUtility.BeginPreview(renderRect, GUIStyle.none);
            
            //Render
            Camera camera = PreviewRenderUtility.camera;
            camera.backgroundColor = BGColor;
            camera.Render();
            
            //End Preview
            RTexture = camera.targetTexture;
            return PreviewRenderUtility.EndPreview();
        }
        
        public Texture CaptureScene(Vector2Int size, Color BGColor)
        {
            return CaptureScene(size, BGColor, out RenderTexture RTexture);
        }
        
        public Texture CaptureScene(Vector2Int size)
        {
            return CaptureScene(size, cameraBackgroundColor, out RenderTexture RTexture);
        }
        
        public Texture CaptureScene()
        {
            return CaptureScene(new Vector2Int(renderTexture.width, renderTexture.height), cameraBackgroundColor, out RenderTexture RTexture);
        }
        
        public RenderTexture RenderScene(Vector2Int size, Color BGColor)
        {
            CaptureScene(size, BGColor, out RenderTexture RTexture);
            return RTexture;
        }
        
        public RenderTexture RenderScene(Vector2Int size)
        {
            CaptureScene(size, cameraBackgroundColor, out RenderTexture RTexture);
            return RTexture;
        }
        
        public RenderTexture RenderScene()
        {
            CaptureScene(new Vector2Int(renderTexture.width, renderTexture.height), cameraBackgroundColor, out RenderTexture RTexture);
            return RTexture;
        }
    }
}