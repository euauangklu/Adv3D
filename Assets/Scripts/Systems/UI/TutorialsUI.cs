using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GDD
{
    public class TutorialsUI : MonoBehaviour
    {
        [SerializeField] private RectTransform m_parent;
        [SerializeField] private TutorialImagesList m_tutorialImagesList;
        [SerializeField] private Image m_image;
        [SerializeField] private Button m_button;
        [SerializeField] private Slider m_slider;
        [SerializeField] private bool autoCloseEndTutorials;
        [SerializeField] private UnityEvent m_OnClose;
        [SerializeField] private UnityEvent m_OnEndTutorials;
        [SerializeField] private UnityEvent m_onCompleteTutorials;
        [SerializeField] private List<UnityEvent> m_OnTutorialsTrigger;
        [SerializeField] private bool isCanShow = true;
        private int _currentTutorials = 0;
        private int currentTrigger = 0;

        private void Awake()
        {
            if (m_tutorialImagesList.isShowed)
                m_onCompleteTutorials?.Invoke();
        }

        private void OnEnable()
        {
            if (!isCanShow || (autoCloseEndTutorials && m_tutorialImagesList.isShowed))
            {
                m_OnClose?.Invoke();
                return;
            }

            if (!m_tutorialImagesList.isShowed)
                OnShow();
            else
                this.enabled = false;
        }

        public void SetShow(bool isShow)
        {
            isCanShow = isShow;
        }

        public void OnShow()
        {
            SelectTutorials(0);
        }
        
        public void SelectTutorials(int index)
        {
            if (m_tutorialImagesList.tutorials.Count <= 1)
            {
                m_tutorialImagesList.isShowed = true;
                m_OnEndTutorials?.Invoke();
            }
            
            _currentTutorials += index;
            
            if (_currentTutorials <= 0)
                _currentTutorials = 0;
            else if (_currentTutorials >= m_tutorialImagesList.tutorials.Count - 1)
            {
                m_OnEndTutorials?.Invoke();
                
                if (_currentTutorials == m_tutorialImagesList.tutorials.Count && autoCloseEndTutorials)
                {
                    m_OnClose?.Invoke();
                    return;
                }
                else
                {
                    _currentTutorials = m_tutorialImagesList.tutorials.Count - 1;
                }
                
                m_tutorialImagesList.isShowed = true;
            }

            print($"Index : {index} || Current : {_currentTutorials}");
            
            //Get Tutorials Data
            KeyValuePair<Sprite, TutorialImagesList.TutorialsSetting> tutorialsData = m_tutorialImagesList.tutorials.ElementAt(_currentTutorials);
            
            //Set Tutorials Images
            Sprite tutorial = tutorialsData.Key;
            
            //Set Anchors
            TutorialImagesList.FitTo _fitTo = tutorialsData.Value.fitTo;
            if (_fitTo == TutorialImagesList.FitTo.Top || _fitTo == TutorialImagesList.FitTo.Bottom)
            {
                m_parent.anchorMax = new Vector2(1, _fitTo.GetHashCode());
                m_parent.anchorMin = new Vector2(0, _fitTo.GetHashCode());
                m_parent.pivot = new Vector2(0.5f, _fitTo.GetHashCode());
                m_parent.anchoredPosition = new Vector2(0, 0);
            }
            else
            {
                m_parent.anchorMax = new Vector2(_fitTo.GetHashCode(), 1);
                m_parent.anchorMin = new Vector2(_fitTo.GetHashCode(), 0);
                m_parent.pivot = new Vector2(_fitTo.GetHashCode(), 0.5f);
                m_parent.anchoredPosition = new Vector2(0, 0);
            }
            
            //Show Tutorials Highlight\
            bool canTrigger = tutorialsData.Value.canHighlight;
            if (canTrigger)
            {
                if (currentTrigger < m_OnTutorialsTrigger.Count)
                {
                    m_OnTutorialsTrigger[currentTrigger]?.Invoke();
                    currentTrigger++;
                }
            }

            //Set Current Tutorials Count
            m_slider.value = _currentTutorials / (m_tutorialImagesList.tutorials.Count - 1.0f);
            SwitchImageMode(tutorial);
        }

        public void SwitchImageMode(Sprite image)
        {
            m_image.sprite = image;
        }

        private void OnDisable()
        {
            if(!autoCloseEndTutorials)
                m_OnClose?.Invoke();
        }
    }
}