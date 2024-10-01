using System;
using UnityEngine;

namespace GDD
{
    public class PauseAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private void Start()
        {
            if(_animator == null) 
                _animator = GetComponent<Animator>();
        }

        public void OnPauseAnimation(bool isPause)
        {
            _animator.speed = isPause ? 0 : 1;
        }
    }
}