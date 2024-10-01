using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class CountingTutorials : MonoBehaviour
    {
        [SerializeField] private List<TutorialImagesList> m_tutorialImagesLists;
        [SerializeField] private UnityEvent m_OnUnlock;
        private bool isUnlocked;
        
        private void Update()
        {
            if(!isUnlocked)
                if (GetAllUnlock())
                {
                    m_OnUnlock?.Invoke();
                    isUnlocked = true;
                }
        }

        private bool GetAllUnlock()
        {
            bool isUnlock = false;
            foreach (var tutorialImagesList in m_tutorialImagesLists)
            {
                isUnlock = tutorialImagesList.isShowed;
                
                if(!isUnlock)
                    break;
            }
            
            return isUnlock;
        }
    }
}