using System;
using System.Collections.Generic;
using System.Linq;
using GDD.Serialize;
using SolidUtilities.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewTutorialImagesList", menuName = "TutorialImagesList", order = 3)]
    public class TutorialImagesList : ScriptableObjectSerialize
    {
        [SerializeField] 
        private SerializableDictionary<Sprite, TutorialsSetting> m_tutorialsImages;
            
        [SerializeField] private bool _isShowed;
        
        public SerializableDictionary<Sprite, TutorialsSetting> tutorials
        {
            get => m_tutorialsImages;
            set => m_tutorialsImages = value;
        }

        [Serializable]
        public enum FitTo
        {
            Top = 1,
            Bottom = 0,
            Left = 1,
            Right = 0
        }
        [Serializable]
        public struct TutorialsSetting
        {
            public FitTo fitTo;
            public bool canHighlight;

            public TutorialsSetting(FitTo _fitTo, bool _canHighlight)
            {
                fitTo = _fitTo;
                canHighlight = _canHighlight;
            }
        }
        
        public bool isShowed
        {
            get => _isShowed;
            set
            {
                _isShowed = value;
                OnSave();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnAssetsLoad(object[] data)
        {
            _isShowed = (bool)data[0];
        }

        protected override object[] GetData()
        {
            return new object[] { _isShowed };;
        }

        private void OnSave()
        {
            OnSerialize(GetData());
        }

        public override void SaveDefaultValue()
        {
            base.SaveDefaultValue();

            _isShowed = false;
            OnSerialize(new object[]{_isShowed});
        }
    }
}