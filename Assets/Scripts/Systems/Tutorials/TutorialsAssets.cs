using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewTutorialsAsset", menuName = "TutorialsAsset", order = 3)]
    public class TutorialsAssets : ScriptableObject
    {
        [SerializeField] private string m_title;
        [SerializeField] private Sprite m_tutorialsImage;
        [SerializeField] [TextArea(20,50)] private string m_tutorials;

        public string title
        {
            get => m_title;
        }
        
        public Sprite tutorialsImage
        {
            get => m_tutorialsImage;
        }

        public string tutorials
        {
            get => m_tutorials;
        }
    }
}