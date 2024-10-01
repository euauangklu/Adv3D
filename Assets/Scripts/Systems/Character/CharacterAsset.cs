using System;
using System.IO;
using GDD.Serialize;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace GDD
{
    public class CharacterAsset : ScriptableObjectSerialize
    {
        [ToggleGroup("ShowAdvancedSettings")] public bool ShowAdvancedSettings;
        [ToggleGroup("ShowAdvancedSettings")][SerializeField] private string m_theme;
        [ToggleGroup("ShowAdvancedSettings")][SerializeField] private bool m_isUnlock;
        [ToggleGroup("ShowAdvancedSettings")][SerializeField] private bool m_isRead;
        [ToggleGroup("ShowAdvancedSettings")][SerializeField] private Sprite m_unlockIcon;
        [ToggleGroup("ShowAdvancedSettings")][SerializeField] private Sprite m_lockIcon;
        [ToggleGroup("ShowAdvancedSettings")][SerializeField][TextArea(5, 20)] private string m_assetsDescription;
        
        [SerializeField] protected Sprite m_icon;
        [SerializeField] protected string m_name;
        [SerializeField] private string m_characterClothing;

        public bool isUnlock
        {
            get => m_isUnlock;
            set
            {
                m_isUnlock = value;
                OnSave();
            } 
        }

        public bool isRead
        {
            get => m_isRead;
            set
            {
                m_isRead = value; 
                OnSave();
            }
        }

        public string clothing
        {
            get => m_characterClothing;
        }
        
        public string assetName
        {
            get => m_name;
        }

        public Sprite icon
        {
            get => m_icon;
        }

        public Sprite unlockIcon
        {
            get => m_unlockIcon;
        }

        public Sprite lockIcon
        {
            get => m_lockIcon;
        }

        public string assetsDescription
        {
            get => m_assetsDescription;
        }

        public string theme
        {
            get => m_theme;
        }

        protected override void OnEnable()
        {
            fileName = name;
            
            base.OnEnable();
        }

        protected override void OnAssetsLoad(object[] data)
        {
            m_isRead = (bool)data[0];
            m_isUnlock = (bool)data[1];
        }

        protected override object[] GetData()
        {
            return new object[]
            {
                m_isRead,
                m_isUnlock
            };
        }

        private void OnSave()
        {
            OnSerialize(GetData(), name);
        }

        public override void SaveDefaultValue()
        {
            base.SaveDefaultValue();
            m_isRead = false;
            m_isUnlock = false;
            OnSerialize(new object[]{m_isRead,
                m_isUnlock});
        }
    }
}