using System;
using UnityEngine;

namespace GDD
{
    public class EasterEgg : MonoBehaviour
    {
        [SerializeField] private Animator m_Maleanimator;
        [SerializeField] private Animator m_Femaleanimator;
        [SerializeField] private AnimationClip m_animEasterEgg;
        [SerializeField] private AudioSource m_audioSource;
        [SerializeField] private AudioClip m_music;
        [SerializeField] private int m_numberEgg;
        private int _currentNumberTouch;
        private int _currentNumberToPlayMusic;

        private void OnEnable()
        {
            _currentNumberTouch = 0;
            _currentNumberToPlayMusic = 0;
        }
        
        public void AddEgg()
        {
            _currentNumberTouch++;
            if (_currentNumberTouch == m_numberEgg)
            {
                _currentNumberToPlayMusic++;

                if (_currentNumberToPlayMusic == m_numberEgg)
                {
                    m_audioSource.clip ??= m_music;
                    m_audioSource.Stop();
                    m_audioSource.Play();
                    BackgroundMusicManager.Instance.Pause();
                    _currentNumberToPlayMusic = 0;
                }
                
                m_Maleanimator.SetTrigger(m_animEasterEgg.name);
                m_Femaleanimator.SetTrigger(m_animEasterEgg.name);
                _currentNumberTouch = 0;
            }
        }

        private void Update()
        {
            if(!m_audioSource.isPlaying && !BackgroundMusicManager.Instance.play)
                BackgroundMusicManager.Instance.Play();
        }
    }
}