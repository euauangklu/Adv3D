using UnityEngine;

namespace KongC
{
    public class PreviewRoomRuntime
    {
        private PreviewRenderRuntimeUtility PRRU;
        public Color cameraBackgroundColor = new Color(0,0,0,0);
        public Vector3 cameraPosition = new Vector3(0, 0, -1.5f);
        public RenderTexture renderTexture = new RenderTexture(1920, 1080, 32);
        public CameraClearFlags cameraClearFlags = CameraClearFlags.SolidColor;
        public bool orthographic = false;
        public float fieldOfView = 60f;
        public float orthographicSize = 1f;
        public float nearClipPlane = 0.01f;
        public Vector3 lightRotation = Vector3.zero;
        public Vector3 lightPosition = new Vector3(0, 0, -5);
        public float lightRange = 100;
        public float lightIntensity = 5;
        private GameObject prefabInScene;
        
        public void OnInitialization(Color backgroundColor)
        {
            if(PRRU == null)
                InitializeScene(backgroundColor);
        }

        public PreviewRenderRuntimeUtility previewRenderRuntimeUtility
        {
            get => PRRU;
        }
        
        private void InitializeScene(Color backgroundColor)
        {
            PRRU = new PreviewRenderRuntimeUtility(backgroundColor);
            
            //Setting Camera
            Camera camera = PRRU.camera;
            camera.backgroundColor = cameraBackgroundColor;
            camera.transform.position = cameraPosition;
            camera.orthographic = orthographic;
            camera.fieldOfView = fieldOfView;
            camera.nearClipPlane = nearClipPlane;
            camera.orthographicSize = orthographicSize;
            camera.clearFlags = cameraClearFlags;
            
            //Setting Anti-aliasing
            renderTexture.antiAliasing = 1;
            camera.targetTexture = renderTexture;
            
            //Setting Light
            PRRU.AddLightInScene();
            PRRU.lights[0].transform.rotation = Quaternion.Euler(lightRotation);
            PRRU.lights[0].transform.position = lightPosition;
            PRRU.lights[0].range = lightRange;
            PRRU.lights[0].intensity = lightIntensity;
        }

        public GameObject AddGameObjectInScene(GameObject ObjectGo, Vector3 position)
        {
            return PRRU.AddGameObjectInScene(ObjectGo, position);
        }
        
        public void SetLookAt(Vector3 position)
        {
            PRRU.SetLookAt(position);
        }
        
        public void SetCameraPosition(Vector3 position)
        {
            PRRU.camera.transform.position = position;
        }
        
        public Texture2D StartPreview(Vector2Int size)
        {
            Texture2D previewTexture = new Texture2D(size.x, size.y, TextureFormat.RGBA32, false);
            return PRRU.RenderPreview(previewTexture);
        }
        
        public void Destroy(Object unityObject)
        {
            GameObject.Destroy(unityObject);
        }
    }
}