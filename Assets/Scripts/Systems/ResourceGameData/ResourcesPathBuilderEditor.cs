using UnityEditor;
using UnityEngine;

namespace GDD
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ResourcesPathBuilderScript))]
    public class ResourcesPathBuilderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.LabelField("Create ScriptableObject Path Resources");
            ResourcesPathBuilderScript builderScript = (ResourcesPathBuilderScript)target;
            if (GUILayout.Button("Create"))
            {
                builderScript.BuildResourcesFile();
            }
        }
    }
#endif
}