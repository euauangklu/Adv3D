using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GDD
{
    public class MassageUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_title;
        [SerializeField] private TextMeshProUGUI m_massage;
        [SerializeField] private Button m_buttonCancel;
        [SerializeField] private TextMeshProUGUI m_cancelText;
        [SerializeField] private Button m_buttonOK;
        [SerializeField] private TextMeshProUGUI m_acceptText;

        private UnityAction _cancelAction;
        private UnityAction _okAction;

        public UnityAction cancelAction
        {
            set => m_buttonCancel.onClick.AddListener(value);
        }

        public UnityAction okAction
        {
            set =>  m_buttonOK.onClick.AddListener(value);
        }

        public string titleText
        {
            set => m_title.text = value;
        }

        public string acceptText
        {
            set => m_acceptText.text = value;
        }
        
        public string cancelText
        {
            set => m_cancelText.text = value;
        }

        public string massageText
        {
            set => m_massage.text = value;
        }

        public int sortingOrder
        {
            set => GetComponent<Canvas>().sortingOrder = value;
        }

        public void SwitchButton()
        {
            m_buttonCancel.transform.parent.SetSiblingIndex(1);
            m_buttonOK.transform.parent.SetSiblingIndex(0);
        }

        public MassageUI(UnityAction cancel, UnityAction ok, string title, string massage, int sortingOrder = 0)
        { 
            m_buttonCancel.onClick.AddListener(cancel);
            m_buttonOK.onClick.AddListener(ok);
            m_title.text = title;
            m_massage.text = massage;
            GetComponent<Canvas>().sortingOrder = sortingOrder;
        }

        private void Start()
        {
            m_buttonCancel.onClick.AddListener(() =>
            {
                Destroy(gameObject);
            });
            m_buttonOK.onClick.AddListener(() =>
            {
                Destroy(gameObject);
            });
        }
    }
}