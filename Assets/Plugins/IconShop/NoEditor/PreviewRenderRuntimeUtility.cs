using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace KongC
{
    public class PreviewRenderRuntimeUtility
    {
        private PreviewSceneStandalone previewScene;
        private RenderTexture renderTexture;

        public Camera camera
        {
            get => previewScene.camera;
        }

        public List<Light> lights
        {
            get => previewScene.lights;
            set => previewScene.lights = value;
        }

        public PreviewRenderRuntimeUtility(Color backgroundColor)
        {
            previewScene = new PreviewSceneStandalone(backgroundColor);
        }

        public Texture2D RenderPreview(Texture2D previewTexture)
        {
            renderTexture = new RenderTexture(previewTexture.width, previewTexture.height, 24);
            
            // Render the GameObject
            previewScene.Render(renderTexture);
            RenderTexture.active = renderTexture;
            previewTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            previewTexture.Apply();
            RenderTexture.active = null;

            return previewTexture;
        }

        public GameObject AddGameObjectInScene(GameObject ObjectGo, Vector3 position)
        {
            return previewScene.AddGameObject(ObjectGo, position);
        }

        public void SetLookAt(Vector3 position)
        {
            previewScene.SetCameraLookAtPosition(position);
        }

        public void AddLightInScene()
        {
            previewScene.AddLight();
        }

        public void Dispose()
        {
            previewScene.Dispose();
            renderTexture.Release();
        }
    }
}