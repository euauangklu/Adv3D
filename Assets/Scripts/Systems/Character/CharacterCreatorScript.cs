
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using StarterAssets;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace GDD
{
    public class CharacterCreatorScript : MonoBehaviour
    {
        [Header("Character")] 
        [SerializeField, ]
        private SerializedDictionary<string, CharacterClothing> _characterClothings = new SerializedDictionary<string, CharacterClothing>();
        
        [SerializeField] 
        private AssetsInputs _assetsInputs;
        
        [Header("Input")] 
        [SerializeField] private float _rotateSpeed = 1;

        [Header("Animation")] 
        [SerializeField] private AnimationClip _ReactingAnimation;

        private TouchToPlayAnimationUI _touchToPlay;
        private Tuple<AnimationClip, string> _oldGrabAnimationClip = new Tuple<AnimationClip, string>(null, default);
        private Vector3 _oldFootPos;
        
        public SerializedDictionary<string, CharacterClothing> characterElements
        {
            get => _characterClothings;
        }

        public List<string> defaultOutfit
        {
            get => _characterClothings.Select(a => a.Value._defaultOutfit).ToList();
        }

        public CreatorState creatorState
        {
            get => _creatorState;
            set
            {
                /*if (value == CreatorState.Enable)
                    _touchToPlay.SetAnim = _ReactingAnimation;
                else 
                    _touchToPlay.ResetDefaultAnimation();*/
                
                _creatorState = value;
            }
        }

        public Animator animator
        {
            get => _animator;
        }
        
        //System
        private GameManager GM;
        private CharacterInstance instance;
        private Animator _animator;
        private CreatorState _creatorState;
        
        private void Awake()
        {
            GM = GameManager.Instance;
            _animator = GetComponent<Animator>();
            _oldFootPos = transform.position;
        }

        private void Start()
        {
            _assetsInputs = GetComponent<AssetsInputs>();
            _touchToPlay = GetComponent<TouchToPlayAnimationUI>();
        }

        private void Update()
        {
            if (!PointerOverUIElement.OnPointerOverUIElement())
                gameObject.transform.Rotate(new Vector3(0, _assetsInputs.rotateDelta.x, 0) * _rotateSpeed);
        }
/*
        private void OnInitialize()
        {
            //Set Default Character Element
            foreach (var characterElements in _characterElements)
            {
                CharacterElement elements = characterElements.Value;
                switch (elements._settingComponent)
                {
                    case SettingComponent.MeshFilter:
                        elements = new CharacterElement(
                            elements._settingComponent,
                            elements._characterElement,
                            elements._characterElement.GetComponent<MeshFilter>().sharedMesh
                        );
                        break;
                    case SettingComponent.Material_MeshRenderer:
                        elements = new CharacterElement(
                            elements._settingComponent,
                            elements._characterElement,
                            elements._characterElement.GetComponent<MeshRenderer>().sharedMaterial
                        );
                        break;
                    case SettingComponent.Texture_MeshRenderer:
                        elements = new CharacterElement(
                            elements._settingComponent,
                            elements._characterElement,
                            elements._characterElement.GetComponent<MeshRenderer>().sharedMaterial
                                .GetTexture("_BaseMap")
                        );
                        break;
                }
            }
        }
*/
        public void SetCharacterElement(string elementType, CharacterAsset asset, string assetName, string properties = "_BaseMap")
        {
            instance = GM.characterInstance;
            print($"Type : {elementType} || {assetName}");
            
            //Play Animation
            if(_animator)
                _animator.SetTrigger(_ReactingAnimation.name);
            
            string[] assetsNames = assetName.Split("_");
            string elementTypeFile = assetsNames.Length <= 2 ? elementType : 
                (assetsNames.Length > 3 
                    ? $"{elementType}_{assetsNames[2]}_{assetsNames[3]}" : 
                    $"{elementType}_{assetsNames[2]}");

            if (String.IsNullOrEmpty(assetName))
            {
                instance.characterWardrobe.Remove(elementTypeFile);
            } else if (!instance.characterWardrobe.TryAdd(elementTypeFile, assetName))
            {
                instance.characterWardrobe[elementTypeFile] = assetName;
            }

            if(asset != null)
                print($"Asset Name : {((Object)asset).name}");
            else 
                print($"Asset type : {elementTypeFile}");
            
            CharacterClothing clothing = _characterClothings[elementTypeFile];

            if (assetName == clothing._defaultOutfit || String.IsNullOrEmpty(assetName))
            {
                clothing.showAdvancedSettings._OnUnEquip?.Invoke(asset);
            }
            else
            {
                clothing.showAdvancedSettings._OnEquip?.Invoke(asset);
            }
            
            SetElementType(clothing, asset, elementTypeFile, properties);
        }

        public void SetElementType(CharacterClothing clothing, CharacterAsset asset, string assetName, string properties = "_BaseMap")
        {
            switch (clothing._settingComponent)
            {
                case SettingComponent.MeshFilter:
                    print("Set Mesh");
                    foreach (var charElement in clothing._characterClothingRef)
                    { 
                        SetMeshFilter((CharacterMeshAsset)asset, charElement);
                    }
                    break;
                case SettingComponent.SkinMeshRenderer:
                    print("Set SkinMesh");
                    foreach (var charElement in clothing._characterClothingRef)
                    {
                        SetSkinMeshRenderer((CharacterMeshAsset)asset, charElement);
                    }
                    break;
                case SettingComponent.MeshFilterControlRig:
                    print("Set MeshFilterControlRig");
                    foreach (var charElement in clothing._characterClothingRef)
                    {
                        SetMeshFilterControlRig((CharacterMeshAsset)asset, charElement, clothing.showAdvancedSettings.clothingSide, assetName);
                    }
                    break;
                case SettingComponent.MaterialMesh:
                    print("Set Material");
                    foreach (var charElement in clothing._characterClothingRef)
                    {
                        SetMaterialMesh((CharacterMaterialAsset)asset, charElement);
                    }
                    break;
                case SettingComponent.MaterialSkinMesh:
                    print("Set Material Skin Mesh");
                    foreach (var charElement in clothing._characterClothingRef)
                    {
                        SetMaterialSkinMesh((CharacterMaterialAsset)asset, charElement);
                    }
                    break;
                case SettingComponent.Texture:
                    print("Set Texture");
                    foreach (var charElement in clothing._characterClothingRef)
                    {
                        SetTexture((CharacterTextureAsset)asset, charElement, properties);
                    }
                    break;
                case SettingComponent.Color:
                    print("Set Color");
                    foreach (var charElement in clothing._characterClothingRef)
                    {
                        SetColor((CharacterTextureAsset)asset, charElement, properties);
                    }
                    break;
            }
        }

        public void ResetFootPosition(CharacterAsset asset)
        {
            if (asset != null)
            {
                transform.position = ((CharacterMeshAsset)asset).offsetFoot;
            }
            else
            {
                ResetFootOffset();
            }
        }
        
        private void SetMeshFilter(CharacterMeshAsset asset, GameObject element)
        {
            if (asset != null)
            {
                element.GetComponent<MeshFilter>().sharedMesh = asset.mesh;
                element.GetComponent<MeshRenderer>().sharedMaterials = asset.materials;
            }
            else
            {
                Debug.LogWarning($"Is Mesh Null Assets : {element.name}");
                element.GetComponent<MeshFilter>().sharedMesh = null;
                element.GetComponent<MeshRenderer>().sharedMaterials = new Material[]{null};
            }
        }
        
        public void ResetFootOffset()
        {
            Debug.LogWarning($"Reset Foot Offset");
            transform.position = _oldFootPos;
        }

        private void SetSkinMeshRenderer(CharacterMeshAsset asset, GameObject element)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = element.GetComponent<SkinnedMeshRenderer>();
            
            if (asset != null)
            {
                skinnedMeshRenderer.sharedMesh = asset.mesh;
                skinnedMeshRenderer.sharedMaterials = asset.materials;
            }
            else
            {
                skinnedMeshRenderer.sharedMesh = null;
                skinnedMeshRenderer.sharedMaterials = new Material[]{null};
            }
        }

        private void SetMeshFilterControlRig(CharacterMeshAsset asset, GameObject element, ClothingSide side, string type)
        {
            Debug.LogWarning($"Type ===== { type}");
            MeshFilterControlRig meshFilterControlRig = element.GetComponent<MeshFilterControlRig>();
            CharacterMeshControlRigAsset CMCR = (CharacterMeshControlRigAsset)asset;

            if (asset != null)
            {
                if (side == ClothingSide.Left)
                {
                    foreach (GameObject _element in _characterClothings[type]._characterClothingRef)
                    {
                        _element.GetComponent<MeshFilterControlRig>().setEnableRig = false;
                        SetMeshFilter(null, _element);
                    }
                }
                else if (side == ClothingSide.Right)
                {
                    foreach (GameObject _element in _characterClothings[type]._characterClothingRef)
                    {
                        _element.GetComponent<MeshFilterControlRig>().setEnableRig = false;
                        SetMeshFilter(null, _element);
                    }
                }

                if (_oldGrabAnimationClip.Item1 != null)
                {
                    _animator.SetBool($"is{_oldGrabAnimationClip.Item1.name}", false);
                }

                if (CMCR.grabAnimation != null)
                {
                    
                    if (side == ClothingSide.Left)
                    {Debug.Log($"Hi is Grab Left!!!!!");
                        _animator.SetLayerWeight(_animator.GetLayerIndex("ArmLeft"), 1);
                    }
                    else if (side == ClothingSide.Right)
                    {Debug.Log($"Hi is Grab Right!!!!!");
                        _animator.SetLayerWeight(_animator.GetLayerIndex("ArmRight"), 1);
                    }
                    
                    _animator.SetBool($"is{CMCR.grabAnimation.name}", true);
                    _animator.SetTrigger(CMCR.grabAnimation.name);
                }

                _oldGrabAnimationClip = new Tuple<AnimationClip, string>(CMCR.grabAnimation, type);
                
                meshFilterControlRig.setEnableRig = true;
                meshFilterControlRig.position = CMCR.position;
                meshFilterControlRig.refPosition = CMCR.refPosition;
                meshFilterControlRig.offsetPosition = CMCR.offsetPosition;
                meshFilterControlRig.rotation = CMCR.rotation;
                meshFilterControlRig.refRotation = CMCR.refRotation;
                meshFilterControlRig.offsetRotation = CMCR.offsetRotation;
                meshFilterControlRig.size = CMCR.size;
            }
            else
            {
                Debug.LogWarning($"Is Rig Null Assets : {element.name}");
                meshFilterControlRig.setEnableRig = false;
                meshFilterControlRig.rigBuilder.enabled = false;
                if (_oldGrabAnimationClip.Item1 != null)
                {
                    if (side == ClothingSide.Left)
                    {
                        _animator.SetLayerWeight(_animator.GetLayerIndex("ArmLeft"), 0);
                    }
                    else if (side == ClothingSide.Right)
                    {
                        _animator.SetLayerWeight(_animator.GetLayerIndex("ArmRight"), 0);
                    }
                    _animator.SetBool($"is{_oldGrabAnimationClip.Item1.name}", false);
                }
            }
             
            SetMeshFilter(asset, element);
        }

        private void SetTexture(CharacterTextureAsset texture, GameObject element, string properties)
        {
            Material[] materials = element.GetComponent<Renderer>().sharedMaterials;
            
            if(texture == null)
                return;

            if (texture.isAll)
            {
                foreach (var material in materials)
                {
                    material.SetTexture((properties != "_BaseMap" ? properties : texture.nameTextureProperty),
                        texture.texture);
                }
            }
            else
            {
                materials[texture.index].SetTexture((properties != "_BaseMap" ? properties : texture.nameTextureProperty),
                    texture.texture);
            }
        }
        
        private void SetColor(CharacterTextureAsset colorMaterialAsset, GameObject element, string properties)
        {
            Material[] materials = element.GetComponent<Renderer>().sharedMaterials;
            
            if(colorMaterialAsset == null)
                return;
            if (colorMaterialAsset.isAll)
            {
                foreach (var material in materials)
                {
                    material.SetColor((properties != "_BaseMap" ? properties : colorMaterialAsset.nameColorProperty),
                        colorMaterialAsset.color);
                }
            }
            else
            {
                materials[colorMaterialAsset.index].SetColor((properties != "_BaseMap" ? properties : colorMaterialAsset.nameColorProperty),
                    colorMaterialAsset.color);
            }
        }
        
        private void SetMaterialMesh(CharacterMaterialAsset material, GameObject element)
        {
            if (material != null)
                element.GetComponent<MeshRenderer>().sharedMaterials = material.materials;
            else
                element.GetComponent<MeshRenderer>().sharedMaterials = new Material[]{null};
        }

        private void SetMaterialSkinMesh(CharacterMaterialAsset material, GameObject element)
        {
            if (material != null)
                element.GetComponent<SkinnedMeshRenderer>().sharedMaterials = material.materials;
            else
                element.GetComponent<SkinnedMeshRenderer>().sharedMaterials = new Material[]{null};
        }
    }
}