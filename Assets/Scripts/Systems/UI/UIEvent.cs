using System;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class UIEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent m_OnEnable;
        [SerializeField] private UnityEvent m_OnDisable;
        [SerializeField] private UnityEvent m_Update;
        [SerializeField] private UnityEvent m_OnDestroy;

        private void OnEnable()
        {
            m_OnEnable?.Invoke();
        }

        private void Update()
        {
            m_Update?.Invoke();
        }

        private void OnDisable()
        {
            m_OnDisable?.Invoke();
        }

        private void OnDestroy()
        {
            m_OnDestroy?.Invoke();
        }
    }
}