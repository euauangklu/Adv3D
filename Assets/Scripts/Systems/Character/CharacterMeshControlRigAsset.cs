using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace GDD
{
    [CreateAssetMenu(fileName = "CharacterMeshControlRigAsset", menuName = "CharacterAssets/MeshControlRigAsset", order = 3)]
    class CharacterMeshControlRigAsset : CharacterMeshAsset
    {
        [ToggleGroup("ShowAdvancedSettings")][SerializeField] private Vector3 m_position;
        [ToggleGroup("ShowAdvancedSettings")][SerializeField] private Vector3 m_refPosition;
        [ToggleGroup("ShowAdvancedSettings")][SerializeField] private Vector3 m_OffsetPosition;
        [ToggleGroup("ShowAdvancedSettings")][SerializeField] private Vector3 m_rotation;
        [ToggleGroup("ShowAdvancedSettings")][SerializeField] private Vector3 m_refRotation;
        [ToggleGroup("ShowAdvancedSettings")][SerializeField] private Vector3 m_offsetRotation;
        [ToggleGroup("ShowAdvancedSettings")][SerializeField] private Vector3 m_size = new Vector3(1,1,1);
        [ToggleGroup("ShowAdvancedSettings")][SerializeField] private AnimationClip _grabAnimation;

        public Vector3 position
        {
            get => m_position;
        }
        
        public Vector3 refPosition
        {
            get => m_refPosition;
        }
        
        public Vector3 offsetPosition
        {
            get => m_OffsetPosition;
        }
        
        public Vector3 rotation
        {
            get => m_rotation;
        }

        public Vector3 refRotation
        {
            get => m_refRotation;
        }
        
        public Vector3 offsetRotation
        {
            get => m_offsetRotation;
        }
        
        public Vector3 size
        {
            get => m_size;
        }

        public AnimationClip grabAnimation
        {
            get => _grabAnimation;
        }
    }
}