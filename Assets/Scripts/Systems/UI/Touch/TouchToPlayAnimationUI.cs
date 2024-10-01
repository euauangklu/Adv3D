using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GDD
{
    public class TouchToPlayAnimationUI : TouchRaycast
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationClip _animationClipPlay;
        private bool _isPlay;
        private AnimationClip _oldAnimation;

        public AnimationClip SetAnim
        {
            set
            {
                if (_oldAnimation == null)
                    _oldAnimation = _animationClipPlay;

                _animationClipPlay = value;
            }
        }

        public void ResetDefaultAnimation()
        {
            _animationClipPlay = _oldAnimation;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void Start()
        {
            base.Start();
            if (_animator == null)
                _animator = GetComponent<Animator>();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void OnTouch()
        {
            base.OnTouch();
            _animator.SetTrigger(_animationClipPlay.name);
        }
    }
}