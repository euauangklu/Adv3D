using System;
using System.IO;
using GDD.Serialize;
using UnityEngine;

namespace GDD
{
    public class TermsPrivacyUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_hideObject;
        [SerializeField] private bool m_dontHide;
        [SerializeField] private bool isAccept;
        protected SaveManager SM;
        protected string fileName;

        protected virtual void OnEnable()
        {
            SM ??= SaveManager.Instance;
            SM.onClearData += OnDefault;
            object[] data = OnDeSerialize();
            Debug.Log($"OnEnable!!! Data is : {data}"); 
            
            if (data == null)
            {
                Debug.Log($"isNull : name : {fileName}");
                data = OnSerialize(GetData());
                return;
            }
            else
            {
                Debug.Log($"OnEnable!!! Data is : {data}");
            }
            
            OnAssetsLoad(data);

            if (!m_dontHide && isAccept)
                m_hideObject.SetActive(false);
        }

        public void OnAccept(bool value)
        {
            isAccept = value;
            OnSave();
        }
        
        protected void OnAssetsLoad(object[] data)
        {
            isAccept = (bool)data[0];
        }

        protected object[] GetData()
        {
            return new object[] { isAccept };
        }
        
        public virtual object[] OnSerialize(object[] value)
        {
            if (String.IsNullOrEmpty(fileName))
                fileName = name;
            
            return OnSerialize(value, fileName);
        }
        
        public virtual object[] OnSerialize(object[] value, string _fileName)
        {
            fileName = _fileName;
            Debug.Log($"Serialize Name is : {fileName}");
            SM ??= SaveManager.Instance;
            string location = Application.persistentDataPath + "/AssetsData";

            if (!Directory.Exists(location))
                Directory.CreateDirectory(location);

            string savePath = location + $"/Assets {_fileName}"; 
            Debug.Log($"Ser Save Path : {savePath}");
            SM.SaveGameObjectData<object[]>(value ,savePath);
            
            return value;
        }
        
        protected virtual void OnSetDefaultValue()
        {
            isAccept = false;
        }
        
        public void OnDefault()
        {
            OnSetDefaultValue();
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
            
            SM = SaveManager.Instance;
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
        
        private void OnSave()
        {
            OnSerialize(GetData());
        }

        private void OnDisable()
        {
            SM ??= SaveManager.Instance;
            SM.onClearData -= OnDefault;
        }
    }
}