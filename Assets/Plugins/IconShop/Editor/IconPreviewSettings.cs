using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace KongC
{
    public class IconPreviewSettings : EditorWindow
    {
        private IconShopManager ICM;
        private bool isShowAdvanceSetting;
        
        [MenuItem("Iconshop/Setting")]
        public static void OnInitialize()
        {
            IconPreviewSettings IPS_Window = GetWindow<IconPreviewSettings>();
            IPS_Window.title = "Setting";
            IPS_Window.titleContent.tooltip = "Setting";
            IPS_Window.autoRepaintOnSceneChange = true;
            IPS_Window.Show();
        }

        private void OnEnable()
        {
            ICM ??= IconShopManager.Instance;
        }

        private void OnGUI()
        {
            GUILayout.Label("Default Settings", EditorStyles.boldLabel);
            
            ICM.previewPosition = EditorGUILayout.Vector2Field($"Position", ICM.previewPosition);

            ICM.previewZoom = EditorGUILayout.FloatField("Zoom", ICM.previewZoom * 100) / 100;
            ICM.previewZoomSpeed = EditorGUILayout.Slider("Zoom Speed", ICM.previewZoomSpeed * 100, 0.1f, 100) / 100;

            // Add a Color Field to set the RGB color
            ICM.defaultCameraBGColor = EditorGUILayout.ColorField("Preview Background Color", ICM.defaultCameraBGColor);
            
            // Change Resolution
            ICM.SetResolution(EditorGUILayout.Vector2Field("Resolution", ICM.resolution));
            
            // Change Aspect Ratio
            ICM.SetAspectRatio((AspectRatio)EditorGUILayout.Popup("Aspect Ratio", (int)ICM.aspectRatio, (ICM.orientation == Orientation.Landscape ? ICM.aspectRatioLandscape.Values.ToArray() : ICM.aspectRatioPortrait.Values.ToArray())));
            
            // Change Aspect Orientation
            ICM.SetOrientation((Orientation)EditorGUILayout.Popup("Orientation", (int)ICM.orientation, Enum.GetNames(typeof(Orientation))));
            
            //Advance Setting
            Rect advanceRect = EditorGUILayout.BeginVertical("Foldout");

            if (GUI.Button(advanceRect, GUIContent.none))
                isShowAdvanceSetting =! isShowAdvanceSetting;
            
            GUILayout.Label("Advance Setting");
            if (isShowAdvanceSetting)
            {
                ICM.isShowPreviewInfo = GUILayout.Toggle(ICM.isShowPreviewInfo, "Show Preview Info");
            }
            EditorGUILayout.EndVertical();
            
            /*Rect r = EditorGUILayout.BeginHorizontal("Button");
            if (GUI.Button(r, GUIContent.none))
                Debug.Log("Go here");
            GUILayout.Label("I'm inside the button");
            GUILayout.Label("So am I");
            EditorGUILayout.EndHorizontal();*/
        }
    }
}