using GDD.Sinagleton;
using UnityEngine;

namespace GDD
{
    public class BackgroundMusicManager : CanDestroy_Sinagleton<BackgroundMusicManager>
    {
        private AudioSource _audioSource;
        private bool isPlay = true;

        public bool play
        {
            get => isPlay;
        }
        
        public override void OnAwake()
        {
            base.OnAwake();

            _audioSource = GetComponent<AudioSource>();
        }

        public void Play()
        {
            if (!isPlay)
            {
                _audioSource.Play();
                isPlay = true;
            }

        }

        public void Pause()
        {
            if (isPlay)
            {
                _audioSource.Pause();
                isPlay = false;
            }
        }

        public void SetVolume(float volume)
        {
            _audioSource.volume = volume;
        }
    }
}