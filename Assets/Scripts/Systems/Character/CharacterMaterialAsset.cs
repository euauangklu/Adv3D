using Sirenix.OdinInspector;
using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewCharacterMaterialAsset", menuName = "CharacterAssets/MaterialAsset", order = 3)]
    public class CharacterMaterialAsset : CharacterAsset
    {
        [SerializeField] private Material[] m_materials;

        public Material[] materials
        {
            get => m_materials;
        }
    }
}