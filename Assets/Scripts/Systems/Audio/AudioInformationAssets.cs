using UnityEngine;

namespace Systems.Audio
{
    [CreateAssetMenu(fileName = "NewAudioInformationAssets", menuName = "AudioInformationAssets", order = 4)]
    public class AudioInformationAssets : ScriptableObject
    {
        [SerializeField] private AudioClip m_audioClips;
        [SerializeField] private string _nameCultures;
        [SerializeField][TextArea(10, 50)] private string _dialogs;
        [SerializeField] private Sprite _images;
        [SerializeField][TextArea(10, 50)] private string _URLs;
        [SerializeField] private Sprite m_collectionImage;

        public AudioClip audioClips
        {
            get => m_audioClips;
        }

        public Sprite collectionImage
        {
            get => m_collectionImage;
        }
        
        public string nameCultures
        {
            get => _nameCultures;
        }

        public string dialogs
        {
            get => _dialogs;
        }

        public Sprite images
        {
            get => _images;
        }

        public string URLs
        {
            get => _URLs;
        }
    }
}