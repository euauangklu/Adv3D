using System;
using UnityEditor;
using UnityEngine;

namespace KongC
{
    public class IconPreview : EditorWindow
    {
        protected static GameObject _gameObject;
        protected GameObject _backgroundObj;
        protected static Texture2D _PreviewTexture;
        protected IconShopManager ICM;
        protected PreviewRoom _previewRoom;
        protected Rect sceneRect;
        protected Vector2 _dragStart;
        protected Event _event;
        protected Vector2 scrollPosition;
        protected Texture2D BGPreview;
        
        [MenuItem("Iconshop/Preview")]
        public static void OnInitialize()
        {
            _gameObject ??= Resources.Load<GameObject>("PreviewRoom");
            
            IconPreview IP_Window = GetWindow<IconPreview>();
            IP_Window.title = "Preview";
            IP_Window.titleContent.tooltip = "Preview";
            IP_Window.autoRepaintOnSceneChange = true;
            IP_Window.Show();
        }

        protected virtual void OnEnable()
        {
            ICM ??= IconShopManager.Instance;
            _gameObject ??= Resources.Load<GameObject>("PreviewRoom");

            if (_previewRoom == null)
            {
                _previewRoom = new PreviewRoom()
                {
                    cameraBackgroundColor = ICM.defaultCameraBGColor
                };
                
                _previewRoom.OnInitialization();
            }

            ChangeBackground(_gameObject);
        }

        private void OnGUI()
        {
            if (_gameObject == null)
            {
                EditorGUILayout.LabelField("No Game OJ");
                return;
            }

            BeforeDrawTexture();
            
            // Create Preview Scene
            CreateScene();
            
            AfterDrawTexture();
        }

        private void CreateScene()
        {
            //Begin Preview
            Rect renderRect = new Rect(0, 0, ICM.resolution.x, ICM.resolution.y);
            _previewRoom.PreviewRenderUtility.BeginPreview(renderRect, GUIStyle.none);
            
            //Update Setting Camera
            UpdateCamera();
            
            //Render
            _previewRoom.PreviewRenderUtility.camera.Render();
            //_PreviewTexture = _previewRenderUtility.camera.targetTexture;
            
            //End Preview
            _previewRoom.PreviewRenderUtility.EndPreview();
            
            //Set Handling 
            Vector2 scaleWindow = new Vector2(position.width, position.height);
            Vector2 fitWindow = UtilityPreview.FitInParent(scaleWindow * ICM.previewZoom, ICM.resolution);
            //Vector2 posWindows = UtilityPreview.FixCenter(scaleWindow, Vector2.zero, fitWindow);
            Vector2 posWindows = UtilityPreview.AutoSetPositionOverWindow(scaleWindow, ICM.previewPosition, fitWindow);
            
            sceneRect = new Rect(posWindows.x, posWindows.y, fitWindow.x, fitWindow.y);
            
            // Begin the scroll view
            Vector2 currentScroll = EditorGUILayout.BeginScrollView(scrollPosition,
                GUILayout.Width(position.width),
                GUILayout.Height(position.height));
            
            //Update Handling
            Handling();

            Vector2 drawSize = new Vector2(
                sceneRect.x + sceneRect.width,
                sceneRect.y + sceneRect.height);
            Rect drawTRect = GUILayoutUtility.GetRect(drawSize.x, drawSize.y);
            drawTRect.xMax = drawSize.x;
            drawTRect.yMax = drawSize.y;
            
            scrollPosition = UtilityPreview.AutoScrollCenter(position.size, sceneRect.size, currentScroll);
            
                /*-500;//(sceneRect.position.x < -(position.width / 2)) ? sceneRect.position.x + sceneRect.width : 0;
            drawTRect.xMin = -500;//(sceneRect.position.y < -(position.height / 2)) ? sceneRect.position.y + sceneRect.height : 0;*/
            
            //Draw Preview Texture
            BGPreview = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            BGPreview.SetPixel(0,0, Color.blue);//new Color(0,0,0,0));
            BGPreview.filterMode = FilterMode.Point;
            BGPreview.alphaIsTransparency = true;
            BGPreview.Apply();
            GUI.DrawTexture(drawTRect, BGPreview);
            GUI.DrawTexture(sceneRect, _previewRoom.PreviewRenderUtility.camera.targetTexture);
            
            // End the scroll view
            EditorGUILayout.EndScrollView();
        }

        private void BeforeDrawTexture()
        {
            
        }

        private void AfterDrawTexture()
        {
            if (ICM.isShowPreviewInfo)
            {
                GUIStyle zoomTextStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.UpperLeft,
                    fontSize = 12,
                    normal = { textColor = Color.white },
                    fontStyle = FontStyle.Bold
                };
                GUI.Label(new Rect(10, 10, position.width, 20), $"Zoom : {ICM.previewZoom * 100}%", zoomTextStyle);
                GUI.Label(new Rect(10, 30, position.width, 20),
                    $"Position : (X = {ICM.previewPosition.x}px, Y = {ICM.previewPosition.y}px)", zoomTextStyle);
            }
        }

        private void ChangeBackground(GameObject background)
        {
            if(_backgroundObj != null)
                Destroy(_backgroundObj);
            
            _backgroundObj = _previewRoom.AddPrefabInScene(background);
        }
        
        [MenuItem("Iconshop/Capture")]
        public static void OnCapture()
        {
            CreateRoomPreviewPrefab.GetCaptureScene(_PreviewTexture);
        }

        protected void UpdateCamera()
        {
            Camera camera = _previewRoom.PreviewRenderUtility.camera;
            camera.backgroundColor = ICM.defaultCameraBGColor;
        }

        protected void Handling()
        {
            _event ??= Event.current;

            UtilityPreview.MouseEvent(EventType.ScrollWheel, 0, _event, e =>
            {
                ICM.previewZoom += (e.delta.y * ICM.previewZoomSpeed) * Time.deltaTime;
                e.Use();
            });
            
            UtilityPreview.MouseEvent(EventType.MouseDown, 0, _event, e =>
            {
                _dragStart = e.mousePosition;
            });
            
            UtilityPreview.MouseEvent(EventType.MouseDrag, 0, _event, e =>
            {
                Vector2 dragDelta = e.mousePosition - _dragStart;
                ICM.previewPosition += dragDelta;
                _dragStart = e.mousePosition;
                e.Use();
            });
        }
        
        protected virtual void OnDisable()
        {
            _previewRoom.PreviewRenderUtility.Cleanup();
            _gameObject = null;
        }
    }
}