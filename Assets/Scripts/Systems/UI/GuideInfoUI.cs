using System;
using System.Collections.Generic;
using SolidUtilities.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class GuideInfoUI : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<string, TutorialsAssets> _tutorialsAssets = new SerializableDictionary<string, TutorialsAssets>();
        [SerializeField] private TextMeshProUGUI m_title;
        [SerializeField] private Image m_guideImage;
        [SerializeField] private TextMeshProUGUI m_guideText;

        private string _nameGuide;

        public void SelectGuide(string nameGuide)
        {
            _nameGuide = nameGuide;
        }
        
        private void OnEnable()
        {
            if (_tutorialsAssets == null)
             return;

            m_title.text = _tutorialsAssets[_nameGuide].title;
            m_guideImage.sprite = _tutorialsAssets[_nameGuide].tutorialsImage;
            m_guideText.text = _tutorialsAssets[_nameGuide].tutorials;
        }
    }
}