using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace GDD
{
    public class ChangePostAnimationUI : MonoBehaviour
    {
        [SerializeField] private List<AnimationClip> _animationClip = new List<AnimationClip>();
        private Animator _animator;
        private int currentAnim = 0;
        private Keyboard currentKeyboard;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            currentKeyboard = Keyboard.current;
        }

        private void Update()
        {
            if (currentKeyboard.tKey.wasPressedThisFrame)
                ChangeAnimation(0);
            
            if (currentKeyboard.uKey.wasPressedThisFrame)
                ChangeAnimation(1);
            
            if (currentKeyboard.iKey.wasPressedThisFrame)
                ChangeAnimation(2);
        }

        public void ChangeAnimation(int index)
        {
            _animator.SetTrigger(_animationClip[index].name);
            print("Change Post!!!!!!!!");
        }

        public void NextAnimation(int value)
        {
            currentAnim += value;
            
            _animator.SetTrigger(_animationClip[currentAnim - 1].name);
            
            if (currentAnim > _animationClip.Count - 1)
                currentAnim = 0;
            else if (currentAnim <= 0)
                currentAnim = _animationClip.Count - 1;
        }

        public void Reset()
        {
            currentAnim = 0;
        }
    }
}