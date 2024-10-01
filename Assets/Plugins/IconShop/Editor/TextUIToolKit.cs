using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace KongC
{
    public class TextUIToolKit : IconPreview
    {
        // เพิ่ม UI จาก UXML
        [SerializeField]VisualTreeAsset visualTree;
        private VisualElement _previewTexture;
        
        [MenuItem("Iconshop/Test")]
        public static void ShowUI()
        {
            TextUIToolKit wnd = GetWindow<TextUIToolKit>();
            wnd.titleContent = new GUIContent("My Custom Editor Window");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        private void CreateGUI()
        {
            visualTree.CloneTree(rootVisualElement);
            _previewTexture = rootVisualElement.Q<VisualElement>("Preview");
        }

        private void OnGUI()
        {
            //Begin Preview
            Rect renderRect = new Rect(0, 0, ICM.resolution.x, ICM.resolution.y);
            _previewRoom.PreviewRenderUtility.BeginPreview(renderRect, GUIStyle.none);
            
            //Update Setting Camera
            UpdateCamera();
            
            //Render
            _previewRoom.PreviewRenderUtility.camera.Render();
            RenderTexture renderTexture = _previewRoom.PreviewRenderUtility.camera.targetTexture;
            AsyncGPUReadback.Request(renderTexture, 0, request => 
            {
                if (request.hasError)
                {
                    Debug.LogError("GPU Readback Error");
                    return;
                }

                var data = request.GetData<Color32>();
                _PreviewTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
                _PreviewTexture.SetPixels32(data.ToArray());
                _PreviewTexture.Apply();
            });
            
            
            //End Preview
            _previewRoom.PreviewRenderUtility.EndPreview();
            
            //Set Handling 
            Vector2 scaleWindow = new Vector2(position.width, position.height);
            Vector2 fitWindow = UtilityPreview.FitInParent(scaleWindow * ICM.previewZoom, ICM.resolution);
            //Vector2 posWindows = UtilityPreview.FixCenter(scaleWindow, Vector2.zero, fitWindow);
            Vector2 posWindows = UtilityPreview.AutoSetPositionOverWindow(scaleWindow, ICM.previewPosition, fitWindow);
            
            sceneRect = new Rect(posWindows.x, posWindows.y, fitWindow.x, fitWindow.y);
            
            //Update Handling
            Handling();

            Vector2 drawSize = new Vector2(
                sceneRect.x + sceneRect.width,
                sceneRect.y + sceneRect.height);
            Rect drawTRect = GUILayoutUtility.GetRect(drawSize.x, drawSize.y);
            drawTRect.xMax = drawSize.x;
            drawTRect.yMax = drawSize.y;
            
            //Draw Preview Texture
            BGPreview = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            BGPreview.SetPixel(0,0, Color.blue);//new Color(0,0,0,0));
            BGPreview.filterMode = FilterMode.Point;
            BGPreview.alphaIsTransparency = true;
            BGPreview.Apply();
            _previewTexture.style.backgroundImage = new StyleBackground(_PreviewTexture);
        }
    }
}