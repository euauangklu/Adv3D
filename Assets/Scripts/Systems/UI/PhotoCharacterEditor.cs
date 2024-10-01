using UnityEditor;
using UnityEngine;

namespace GDD
{
#if UNITY_EDITOR
    [CustomEditor(typeof(PhotoCharacter))]
    public class PhotoCharacterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.LabelField("Take Photo Screen Shot");
            PhotoCharacter photoModeScript = (PhotoCharacter)target;
            if (GUILayout.Button("Take Photo"))
            {
                photoModeScript.OnScreenShot();
            }
        }
    }
#endif
}