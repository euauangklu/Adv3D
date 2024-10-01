using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GDD
{
    public class OpenUIButtonControl : MonoBehaviour
    {
        [SerializeField] private Button m_backButton;
        [SerializeField] private MonoBehaviour m_interfaceBackButton;
        [SerializeField] private bool m_isSetToTop = true;
        private IBackButton _IBackButton;
        private GameObject m_targetUI;
        private OpenUIButtonControl target;
        private Button _button;
        private UnityEvent _event = new UnityEvent();

        public Button backButton
        {
            get => m_backButton;
        }

        public IBackButton interfaceBackButton
        {
            get
            {
                if(_IBackButton == null) 
                    _IBackButton = (IBackButton)m_interfaceBackButton;
                
                return _IBackButton;
            }
        }

        public UnityEvent addEvent
        {
            get => _event;
            set => _event = value;
        }

        public bool isSetToTop
        {
            get => m_isSetToTop;
        }

        private void Start()
        {
            if (m_backButton != null)
            {
                m_backButton.onClick.AddListener(() => { _event?.Invoke(); });
            }
            else if(m_interfaceBackButton != null)
            {
                try
                {
                    interfaceBackButton.AddEvent(() => { _event?.Invoke(); 
                        print("Add event to back button"); });
                }
                catch (Exception e)
                {
                    Debug.LogError("Not Fond IButton : " + e);
                    throw;
                }
            }
        }

        public void OpenUI(GameObject targetUI)
        {
            SelectUIOpenControl selectUIOpenControl = targetUI.GetComponent<SelectUIOpenControl>();
            print($"Is Target Null : {selectUIOpenControl == null}");
            if (selectUIOpenControl != null && !selectUIOpenControl.isSetBackButton)
            {
                selectUIOpenControl.onOpen?.Invoke();
                return;
            }

            m_targetUI = selectUIOpenControl == null ? targetUI : selectUIOpenControl.ui;
            
            //Set Canvas Sort Order
            Canvas _canvasTarget = m_targetUI.GetComponent<Canvas>();
            Canvas _canvas = GetComponent<Canvas>();
            if(_canvasTarget != null)
                if (_canvas != null)
                {
                    _canvasTarget.sortingOrder = _canvas.sortingOrder + 1;
                }
                else { }
            else 
                if(_canvas == null)
                    m_targetUI.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            
            //Get UIManager Target
            target = m_targetUI.GetComponent<OpenUIButtonControl>();
            if (target == null)
                target = m_targetUI.AddComponent<OpenUIButtonControl>();
            
            //Set Active Target UI
            m_targetUI.SetActive(true);
            
            //Set On Click Back Button Target UI
            if (target.backButton != null || target.interfaceBackButton != null)
            {
                target.addEvent.RemoveAllListeners();
                target.addEvent.AddListener(OnBack);
            }
        }
        
        private void OnBack()
        {
            gameObject.SetActive(true);

            if (isSetToTop)
            {
                Canvas _canvasTarget = m_targetUI.GetComponent<Canvas>();
                Canvas _canvas = GetComponent<Canvas>();
                int sortOrder;

                if (_canvasTarget != null)
                {
                    sortOrder = _canvasTarget.sortingOrder - 1;

                    if (_canvas != null)
                    {
                        if (sortOrder <= 0)
                            _canvas.sortingOrder = 1;
                        else
                            _canvas.sortingOrder = sortOrder;
                        
                        m_targetUI.GetComponent<Canvas>().sortingOrder = 0;
                    }
                }
                else
                {
                    sortOrder = m_targetUI.transform.GetSiblingIndex() - 1;
                    
                    if (_canvas == null)
                    {
                        if (sortOrder <= 0)
                            transform.SetSiblingIndex(1);
                        else 
                            transform.SetSiblingIndex( sortOrder);
                        
                        m_targetUI.transform.SetSiblingIndex(0);
                    }
                }
            }
        }
    }
}