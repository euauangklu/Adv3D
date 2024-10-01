using System;
using UnityEngine;

namespace GDD
{
    public class PlayAudioOnce : MonoBehaviour
    {
        [SerializeField] private AudioClip m_audioClip;

        public void CreateAudioSource()
        {
            AudioSource _audioSource = new GameObject("Audio Sound").AddComponent<AudioSource>();
            _audioSource.transform.position = Vector3.zero;
            _audioSource.playOnAwake = false;
            _audioSource.clip = m_audioClip;
            _audioSource.Play();
            
            Destroy(_audioSource.gameObject, _audioSource.clip.length);
        }
    }
}