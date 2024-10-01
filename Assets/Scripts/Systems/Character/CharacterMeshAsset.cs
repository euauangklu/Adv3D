using Sirenix.OdinInspector;
using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewCharacterMeshAsset", menuName = "CharacterAssets/MeshAsset", order = 1)]
    public class CharacterMeshAsset : CharacterAsset
    {
        [SerializeField] protected Mesh m_mesh;
        [SerializeField] protected Material[] m_materials;
        [ToggleGroup("ShowAdvancedSettings")][SerializeField] protected Vector3 m_offsetFoot;
        
        public Mesh mesh
        {
            get => m_mesh;
        }

        public Material[] materials
        {
            get => m_materials;
        }

        public Vector3 offsetFoot
        {
            get => m_offsetFoot;
        }
    }
}