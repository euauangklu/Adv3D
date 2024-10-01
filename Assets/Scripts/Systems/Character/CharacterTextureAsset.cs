using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewCharacterTextureAsset", menuName = "CharacterAssets/TextureAsset", order = 2)]
    public class CharacterTextureAsset : CharacterAsset
    {
        [SerializeField] private Texture m_texture;
        [ToggleGroup("ShowAdvancedSettings")][SerializeField] private string m_nameTextureProperty;
        [SerializeField] private Color m_color = Color.white;
        [ToggleGroup("ShowAdvancedSettings")][SerializeField] private string m_nameColorProperty;
        [ToggleGroup("ShowAdvancedSettings")][SerializeField] private bool m_setAll;
        [ToggleGroup("ShowAdvancedSettings")][SerializeField] private int m_index;

        public Texture texture
        {
            get => m_texture;
        }

        public string nameTextureProperty
        {
            get
            {
                if (String.IsNullOrEmpty(m_nameTextureProperty))
                    return "_BaseMap";
                else
                    return m_nameTextureProperty;
            }
        }
        
        public Color color
        {
            get => m_color;
        }

        public string nameColorProperty
        {
            get
            {
                if (String.IsNullOrEmpty(m_nameColorProperty))
                    return "_BaseColor";
                else
                    return m_nameColorProperty;
            }
        }
        
        public bool isAll
        {
            get => m_setAll;
        }

        public int index
        {
            get => m_index;
        }
    }
}