using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GDD
{
    public class TabsUIControl : MonoBehaviour
    {
        [Header("Tab Buttons")]
        [SerializeField] private List<Toggle> m_tabButtons = new List<Toggle>();
        [SerializeField] private List<UnityEvent> m_addEventTab = new List<UnityEvent>();
        
        [Header("Tab Panels")]
        [SerializeField] private List<GameObject> m_panels = new List<GameObject>();

        private int index_active = 0;
        private Dictionary<Button, int> m_tabButtonsData = new (); 
        
        private void Start()
        {
            for (int i = 0; i < m_tabButtons.Count; i++)
            {
                int y = i;
                m_tabButtons[i].onValueChanged.AddListener(
                    value =>
                    {
                        int index = y;
                        ButtonTabEvent(index, value);
                    });

                /*
                m_tabButtonsData.Add(m_tabButtons[i], i);
                Button _button = m_tabButtons[i];
                m_tabButtons[i].onClick.AddListener((() =>
                {
                    ButtonTabEvent(m_tabButtonsData[_button]);
                }));*/
            }
            
        }

        private void ButtonTabEvent(int index, bool value)
        {
            if (!value)
                return;

            print($"Index : {index} || Toggle : {value}");
            m_panels[index].SetActive(true);
            m_addEventTab[index]?.Invoke();
            
            if(index != index_active)
                m_panels[index_active].SetActive(false);
            
            index_active = index;
        }

        public void OnSelectNextTab(int tab)
        {
            int indexSelect = index_active;
            
            if (tab > 0)
            {
                indexSelect++;
                if (indexSelect > m_panels.Count - 1)
                    indexSelect = 0;
            }
            else
            {
                indexSelect--;
                if (indexSelect < 0)
                    indexSelect = m_panels.Count - 1;
            }
            
            ButtonTabEvent(indexSelect, true);
        }
    }
}