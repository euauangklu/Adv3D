using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GDD
{
    [Serializable]
    public struct CharacterClothing
    {
        public SettingComponent _settingComponent;
        public List<GameObject> _characterClothingRef;
        public string _defaultOutfit;
        
        [Toggle("ShowAdvancedSettings")]
        public CharacterElementToggleable showAdvancedSettings;

        public CharacterClothing(SettingComponent settingComponent, List<GameObject> characterClothingRef, string defaultOutfit, UnityEvent<CharacterAsset> OnEquip, UnityEvent<CharacterAsset> OnUnEquip, ClothingSide clothingSide, CharacterElementToggleable advancedSettings, bool enableSubElement)
        {
            showAdvancedSettings = advancedSettings;
            _settingComponent = settingComponent;
            _characterClothingRef = characterClothingRef;
            showAdvancedSettings._OnEquip = OnEquip;
            showAdvancedSettings._OnUnEquip = OnUnEquip;
            _defaultOutfit = defaultOutfit;
            showAdvancedSettings.clothingSide = clothingSide;
            showAdvancedSettings._enableSubElement = enableSubElement;
            
        }
    }
    
    [Serializable, Toggle("ShowAdvancedSettings")]
    public class CharacterElementToggleable
    {
        public bool ShowAdvancedSettings;
        public UnityEvent<CharacterAsset> _OnEquip;
        public UnityEvent<CharacterAsset> _OnUnEquip;
        public ClothingSide clothingSide;
        public bool _enableSubElement;
    }

    [Serializable]
    public enum SettingComponent
    {
        MeshFilter,
        SkinMeshRenderer,
        MeshFilterControlRig,
        MaterialMesh,
        MaterialSkinMesh,
        Texture,
        Color
    }

    [Serializable]
    public enum ClothingSide
    {
        None,
        Left,
        Right
    }

    [Serializable]
    public struct SubClothing
    {
        public string _mainType;
        public string _typeNameFile;
        
        public SubClothing(string mainType, string typeNameFile)
        {
            _mainType = mainType;
            _typeNameFile = typeNameFile;
        }
    }
    
    [Serializable]
    public enum CreatorState
    {
        Enable,
        Disable
    }

    [Serializable]
    public enum Gender
    {
        Male,
        Female
    }

    [Serializable]
    public enum ThaiEthnicCulture
    {
        Indeterminate,
        TaiLue,
        TaiYuan,
        TaiKuen,
        TaiYong,
        TaiYai
    }
}