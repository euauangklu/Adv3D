using System;
using UnityEngine;

namespace GDD
{
    public class AnimationController : MonoBehaviour
    {
        private Animator _animator;
        private string _currentPlay;
        private string _currentStop;
        private bool _isPlay = false;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        public void PlayWithBool(string parameterName)
        {
            if(_currentStop != null)
                _animator.SetBool(_currentStop, true);
            _currentStop = null;
            
            if (parameterName == "")
                parameterName = "Play";

            if(_isPlay)
                return;
            
            _isPlay = true;
            _currentPlay = parameterName;
            _animator.SetBool(parameterName, true);
        }

        public void SetBool(string value = "EX. NameParameters,True")
        {
            if(_animator == null)
                return;
            
            string[] parameter = value.Split(",");
            bool isTrue = bool.Parse(parameter[1]);
            _animator.SetBool(parameter[0], isTrue);
        }

        public void SetTrigger(string parameterName)
        {
            if(_animator == null)
                _animator = GetComponent<Animator>();
            
            if (parameterName == "")
                parameterName = "Play";
            
            Debug.LogError("Play Anim !!!!!!!!!!!!!!");
            print($"PLay Anim : {parameterName} || Null {_animator == null}");
            _animator.SetTrigger(parameterName);
        }
        
        public void PlayWithTrigger(string parameterName)
        {
            if (parameterName == "")
                parameterName = "Play";
            
            if(_isPlay)
                return;
            
            _isPlay = true;
            _animator.SetTrigger(parameterName);
            if(_currentStop != null)
                _animator.SetBool(_currentStop, true);
        }

        public void RewindWithBool(string parameterName)
        {
            if(_currentPlay != null)
                _animator.SetBool(_currentPlay, false);
            _currentPlay = null;

            if (parameterName == "")
                parameterName = "Rewind";
            
            if(!_isPlay)
                return;
            
            _isPlay = false;
            _currentStop = parameterName;
            _animator.SetBool(parameterName, true);
        }
        
        public void RewindWithTrigger(string parameterName)
        {
            if (parameterName == "")
                parameterName = "Rewind";
            
            if(!_isPlay)
                return;
            
            _isPlay = false;
            _animator.SetTrigger(parameterName);
            if(_currentStop != null)
                _animator.SetBool(_currentStop, true);
        }
    }
}