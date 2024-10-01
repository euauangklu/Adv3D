using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GDD
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ScriptableSerializeManager))]
    public class ScriptableSerializeManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.LabelField("Clear All Save Data");
            ScriptableSerializeManager builderScript = (ScriptableSerializeManager)target;
            if (GUILayout.Button("Clear"))
            {
                builderScript.ClearData();
            }
        }
    }
    #endif
}