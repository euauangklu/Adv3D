using UnityEditor;
using UnityEngine;

namespace KongC
{
    public class IconGenerate : EditorWindow
    {
        private IconShopManager ICM;
        
        [MenuItem("Iconshop/Generate")]
        public static void OnInitialize()
        {
            IconGenerate IPS_Window = GetWindow<IconGenerate>();
            IPS_Window.title = "Generate";
            IPS_Window.titleContent.tooltip = "Generate";
            IPS_Window.autoRepaintOnSceneChange = true;
            IPS_Window.Show();
        }

        private void OnEnable()
        {
            ICM ??= IconShopManager.Instance;
        }

        private void OnGUI()
        {
            // Add a Save button to save the current settings
            if (GUILayout.Button("Save Settings"))
            {
                Debug.Log("Save!!!!!!!!!!!!!!!!");
            }
        }
    }
}