using System;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class SwitchToggleBG : MonoBehaviour
    {
        [SerializeField] private Image m_image;
        [SerializeField] private Color m_SelectColor = Color.white;
        [SerializeField] private Color m_UnSelectColor = new Color(0.4901961f, 0.3960784f, 0.2470588f, 1.0f);
        private Toggle _toggle;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
            m_image.color = _toggle.isOn ? m_SelectColor : m_UnSelectColor;
        }

        public void Toggle(bool value)
        {
            m_image.color = value ? m_SelectColor : m_UnSelectColor;
        }
    }
}