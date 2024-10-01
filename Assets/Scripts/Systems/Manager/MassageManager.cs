using System;
using GDD.Sinagleton;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class MassageManager : CanDestroy_Sinagleton<MassageManager>
    {
        [SerializeField] private GameObject m_massageUI;

        public bool isValid
        {
            get => m_massageUI != null;
        }
        
        public GameObject CreateMassage(UnityAction cancel, UnityAction ok, string title, string massage, string acceptText, string cancelText, int sortingOrder = 0, bool isSwitch = false)
        {
            if (m_massageUI == null)
                return null;
            
            MassageUI _massageObject = Instantiate(m_massageUI, Vector3.zero, Quaternion.identity).GetComponent<MassageUI>();
            _massageObject.cancelAction = cancel;
            _massageObject.okAction = ok;
            _massageObject.titleText = title;
            _massageObject.massageText = massage;
            _massageObject.sortingOrder = sortingOrder;
            _massageObject.acceptText = acceptText;
            _massageObject.cancelText = cancelText;
            
            if(isSwitch)
                _massageObject.SwitchButton();
            
            return _massageObject.gameObject;
        }
        
        public GameObject CreateMassage(UnityAction cancel, UnityAction ok, string title, string massage, int sortingOrder = 0, bool isSwitch = false)
        {
            if (m_massageUI == null)
                return null;
            
            MassageUI _massageObject = Instantiate(m_massageUI, Vector3.zero, Quaternion.identity).GetComponent<MassageUI>();
            _massageObject.cancelAction = cancel;
            _massageObject.okAction = ok;
            _massageObject.titleText = title;
            _massageObject.massageText = massage;
            _massageObject.sortingOrder = sortingOrder;
            
            if(isSwitch)
                _massageObject.SwitchButton();

            return _massageObject.gameObject;
        }
    }
}