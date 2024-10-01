using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GDD
{
    public class AnimationSelectUI : CharacterEditor
    {
        [SerializeField] private List<CharacterAnimationAsset> m_animationAssets;
        [SerializeField] private GetCharacterAnimator _animator;
        [SerializeField] private TextMeshProUGUI m_NameAnimationText;
        [SerializeField] private TouchToPlayAnimationUI _touchToPlay;
        private int currentSelect;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _touchToPlay = _animator.animator.GetComponent<TouchToPlayAnimationUI>();
        }

        protected override void Start()
        {
            base.Start();
        }

        public void OnSelect(int value)
        {
            m_animationAssets[currentSelect].AnimationClip.wrapMode = WrapMode.Once;
            
            currentSelect += value;
            if (currentSelect >= m_animationAssets.Count - 1)
            {
                currentSelect = m_animationAssets.Count - 1;
            }else if (currentSelect <= 0)
            {
                currentSelect = 0;
            }
            
            m_animationAssets[currentSelect].AnimationClip.wrapMode = WrapMode.Loop;
            _animator.animator.SetTrigger(m_animationAssets[currentSelect].AnimationClip.name);
            _animator.animator.SetBool("Loop", true);
            _touchToPlay.SetAnim = m_animationAssets[currentSelect].AnimationClip;
            _character.creatorState = CreatorState.Enable;
            m_NameAnimationText.text = m_animationAssets[currentSelect].Name;
        }

        public void OnPlay()
        {
            m_animationAssets[currentSelect].AnimationClip.wrapMode = WrapMode.Loop;
            _animator.animator.SetTrigger(m_animationAssets[currentSelect].AnimationClip.name);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            m_animationAssets[currentSelect].AnimationClip.wrapMode = WrapMode.Once;
            _animator.animator.SetTrigger(m_animationAssets[0].AnimationClip.name);
            _animator.animator.SetBool("Loop", false);
            _character.creatorState = CreatorState.Disable;
        }
    }
}