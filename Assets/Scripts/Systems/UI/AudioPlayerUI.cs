using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GDD
{
    public class AudioPlayerUI : MonoBehaviour
    {
        [SerializeField] private AudioClip m_audioClip;
        [SerializeField] private TextMeshProUGUI m_playTime;
        [SerializeField] private TextMeshProUGUI m_maxTime;
        [SerializeField] private Slider _audioSlider;
        [SerializeField] private Button _infoButton;
        private AudioSource _audioSource;
        private string m_url;
        private float maxTime;

        public AudioClip audioClip
        {
            get => m_audioClip;
            set
            {
                m_audioClip = value;
                
                if(m_audioClip != null)
                    _audioSource.clip = m_audioClip;
                
                maxTime = _audioSource.clip.length;
                m_maxTime.text = ConvertTimeSecondToString(_audioSource.clip.length);
            }
        }

        public string url
        {
            set => m_url = value;
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            maxTime = _audioSource.clip.length;
            m_maxTime.text = ConvertTimeSecondToString(_audioSource.clip.length);
        }

        private void Start()
        {
            //_infoButton.onClick.AddListener(OpenURLButton);
        }

        private void Update()
        {
            _audioSlider.value = _audioSource.time / maxTime;
            m_playTime.text = ConvertTimeSecondToString(_audioSource.time);
            
            if(!_audioSource.isPlaying)
                BackgroundMusicManager.Instance.Play();
        }

        private void OpenURLButton()
        {
            if(!String.IsNullOrEmpty(m_url))
                Application.OpenURL(m_url);
        }

        private string ConvertTimeSecondToString(float time)
        {
            int sec = (int)(time % 60.0f);
            int min = (int)((time / 60.0f) % 60.0f);

            return $"{min.ToString("0")}:{sec.ToString("00")}";
        }
        
        public void PlayAndStopAudio()
        {
            BackgroundMusicManager.Instance.Pause();
            
            if(_audioSource.isPlaying)
                _audioSource.Pause();
            else
            {
                if(m_audioClip != null)
                    _audioSource.clip = m_audioClip;
                
                maxTime = _audioSource.clip.length;
                m_maxTime.text = ConvertTimeSecondToString(_audioSource.clip.length);
                _audioSource.Play();
            }
        }

        private void OnDisable()
        {
            BackgroundMusicManager.Instance.Play();
        }
    }
}