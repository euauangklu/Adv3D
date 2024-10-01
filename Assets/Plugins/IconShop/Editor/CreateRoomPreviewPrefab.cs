using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KongC
{
    public class CreateRoomPreviewPrefab : Editor
    {
        private static Editor gameObjectEditor;
        public static GameObject roomPreviewPrefab;

        private void OnEnable()
        {
            Debug.LogError($"Name is : {target.name}");
        }

        private void OnValidate()
        {
            Debug.LogError($"Name is : {target.name}");
        }

        private void OnSceneGUI()
        {
            Debug.LogError($"Name is : {target.name}");
        }

        [MenuItem("Iconshop/Create Room Preview Prefab")]
        public static void CaptureAll()
        {
            Debug.Log("Capture!");
            GetCaptureScene(null);
        }

        private void OnGUI()
        {

        }

        public static void GetCaptureScene(Texture preview)
        {
            if (preview == null)
            {
                GameObject asset = Resources.Load<GameObject>("PreviewRoom");
                /*AssetPreview.SetPreviewTextureCacheSize(2048);
                Texture2D preview = _instance.RenderStaticPreview(AssetDatabase.GetAssetPath(asset), new[] { asset }, 1024, 1024);
                */

                gameObjectEditor = CreateEditor(asset);
                preview = gameObjectEditor.RenderStaticPreview(AssetDatabase.GetAssetPath(asset), null, 1024, 1024);
            }

            Texture2D _endcodeTexture = new Texture2D(preview.width, preview.height, TextureFormat.RGBA32, false);
            RenderTexture currectRT = RenderTexture.active;
            RenderTexture renderTexture = new RenderTexture(preview.width, preview.height, 32);
            Graphics.Blit(preview, renderTexture);

            RenderTexture.active = renderTexture;
            _endcodeTexture.ReadPixels(new Rect(0,0,renderTexture.width, renderTexture.height), 0, 0);
            _endcodeTexture.Apply();

            RenderTexture.active = currectRT;
            
            byte[] imageByte = _endcodeTexture.EncodeToPNG();
            string userFolderPhotoPath;

            userFolderPhotoPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            string combinePath = Path.Combine(userFolderPhotoPath, "GamePhoto");
            string reTPath = combinePath;
            if (!Directory.Exists(combinePath))
            {
                Directory.CreateDirectory(combinePath);
            }

            combinePath = Path.Combine(combinePath, "256564564646" + ".png");
            File.WriteAllBytes(combinePath, imageByte);
        }
    }
}