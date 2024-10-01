using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewScriptableSerializeManagerAsset", menuName = "ScriptableSerializeManager", order = 4), Serializable]
    public class ScriptableSerializeManager : ScriptableObject
    {
        [SerializeField] private UnityEvent _onClear;
        private ScriptableSerializeManager debug_Instance;
        private static ScriptableSerializeManager s_Instance;
        public static ScriptableSerializeManager instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = Resources.Load<ScriptableSerializeManager>("SSManager");
                }
                
                return s_Instance;
            }
        }

        public UnityEvent onClear
        {
            get => _onClear;
            set => _onClear = value;
        }

        private void OnEnable()
        {
            debug_Instance = instance;
        }

        public void ClearData()
        {
            Debug.LogWarning("Clear All Data!" + name);
            
            if (Application.isPlaying)
            {
                ClearSave(GameManager.Instance.defaultSavePath);
                SaveManager.Instance.onClearData?.Invoke();
            }
            _onClear?.Invoke();
        }
        
        public void ClearSave(string path)
        {
            Directory.Delete(path, true);
        }
    }
}