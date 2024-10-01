using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class SelectUIOpenControl : MonoBehaviour
    {
        [SerializeField]private List<SelectUI> m_UI = new ();
        private int _index = 0;

        public GameObject ui
        {
            get
            {
                print($"Open Page is : {_index} : {m_UI[_index].UI.name}");
                return m_UI[_index].UI; 
            }
        }

        public bool isSetBackButton
        {
            get => m_UI[_index].canBack;
        }

        public UnityEvent onOpen
        {
            get => m_UI[_index].OnOpen;
        }

        public void SelectIndex(int index)
        {
            _index = index;
        }
    }

    [Serializable]
    public class SelectUI
    {
        public GameObject UI;
        public bool canBack;
        public UnityEvent OnOpen;

        public SelectUI(GameObject _UI, bool _canBack, UnityEvent _OnOpen)
        {
            UI = _UI;
            canBack = _canBack;
            OnOpen = _OnOpen;
        }
    }
}