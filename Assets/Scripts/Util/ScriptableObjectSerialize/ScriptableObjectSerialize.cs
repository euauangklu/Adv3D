using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GDD.Serialize
{
    public abstract class ScriptableObjectSerialize : ScriptableObject, IScriptableObjectSerialize
    {
        [ToggleGroup("ScriptableObjectSerializeSettings")]
        public bool ScriptableObjectSerializeSettings;
        [ToggleGroup("ScriptableObjectSerializeSettings")][SerializeField] private bool _isResetDefaultAllValue = true;
        protected SaveManager SM;
        private ScriptableSerializeManager SSM;
        protected string fileName;
        
        protected virtual void OnEnable()
        {
            SSM ??= ScriptableSerializeManager.instance;
            SSM.onClear.AddListener(SaveDefaultValue);
            object[] data = OnDeSerialize();
            
            if (data == null)
            {
                SaveDefaultValue();
                return;
            }
            
            OnAssetsLoad(data);
        }

        protected abstract void OnAssetsLoad(object[] data);

        protected abstract object[] GetData();

        public virtual object[] OnSerialize(object[] value)
        {
            if (String.IsNullOrEmpty(fileName))
                fileName = name;
            
            return OnSerialize(value, fileName);
        }
        
        public virtual object[] OnSerialize(object[] value, string _fileName)
        {
            fileName = _fileName;
            Debug.LogWarning($"Serialize Name is : {fileName}");
            SM ??= SaveManager.Instance;
            string location = Application.persistentDataPath + "/AssetsData";

            if (!Directory.Exists(location))
                Directory.CreateDirectory(location);

            string savePath = location + $"/Assets {_fileName}"; 
            Debug.Log($"Ser Save Path : {savePath}");
            SM.SaveGameObjectData<object[]>(value ,savePath);
            
            return value;
        }

        public virtual object[] OnDeSerialize()
        {
            if (String.IsNullOrEmpty(fileName))
                fileName = name;
            
            return OnDeSerialize(fileName);
        }
        
        public virtual object[] OnDeSerialize(string _fileName)
        {
            Debug.Log($"DeSerialize Name is : {_fileName} ");
            
            SM ??= SaveManager.Instance;
            object[] value;
            string location = Application.persistentDataPath + "/AssetsData";

            if (!Directory.Exists(location))
                Directory.CreateDirectory(location);
            
            string savePath = location + $"/Assets {_fileName}";
            
            Debug.Log($"DeSer Save Path : {savePath}");
            if (!File.Exists(savePath + ".json"))
            {
                Debug.Log($"Not Found Save File");
                return null;
            }

            return SM.LoadGameObjectData<object[]>(savePath);
        }

        public virtual void SaveDefaultValue()
        {
            
        }

        protected virtual void OnDisable()
        {
            SSM ??= ScriptableSerializeManager.instance;
            SSM.onClear.RemoveListener(SaveDefaultValue);
#if UNITY_EDITOR
            if(_isResetDefaultAllValue)
                SaveDefaultValue();
#endif
        }
    }
}