using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewCharacterAnimationAsset", menuName = "CharacterAnimationAsset", order = 2)]
    public class CharacterAnimationAsset : ScriptableObject
    {
        [SerializeField] private string m_name;
        [SerializeField] private AnimationClip m_animationClip;

        public string Name
        {
            get => m_name;
        }

        public AnimationClip AnimationClip
        {
            get => m_animationClip;
        }
    }
}