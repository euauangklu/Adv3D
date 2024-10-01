using System;
using UnityEngine;

namespace GDD
{
    public class CharacterLookAt:MonoBehaviour
    {
        [SerializeField] private Transform m_target;
        [SerializeField] private bool isAwayLookAt = false;

        private void Update()
        {
            if(isAwayLookAt)
                transform.LookAt(m_target);
        }

        public void OnLookAt()
        {
            transform.LookAt(m_target);
        }
    }
}