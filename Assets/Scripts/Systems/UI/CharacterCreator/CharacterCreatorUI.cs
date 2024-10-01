using System;
using System.Collections.Generic;
using System.Linq;
using GDD.FileUtill;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace GDD
{
    [RequireComponent(typeof(RenderCharacter))]
    public class CharacterCreatorUI : CharacterEditor
    {
        [Header("UI Elements")] 
        [SerializeField] private string m_openDefaultPage = "hair";
        [SerializeField] private string m_defaultToggleTab = "Shirt";
        [SerializeField] private TextMeshProUGUI m_CharacterText;
        [SerializeField] private TMP_InputField m_CharacterInputText;
        [SerializeField] private GameObject m_contentScrollView;
        [SerializeField] private GameObject m_contentElement;
        [SerializeField] private List<Toggle> m_contentToggle = new List<Toggle>();
        [SerializeField] private RenderCharacter _renderCharacter;

        [Header("Assets Filter")] 
        [SerializeField] private string m_theme = "";
        [SerializeField] private string m_clothes = "Clothes";
        [SerializeField] private bool ignoreGender = true;
        
        private bool is_sorted = false;
        AsyncOperationHandle<IList<CharacterMeshAsset>> _operationHandleGameObject = new AsyncOperationHandle<IList<CharacterMeshAsset>>();
        AsyncOperationHandle<IList<CharacterMaterialAsset>> _operationHandleMaterial = new AsyncOperationHandle<IList<CharacterMaterialAsset>>();
        AsyncOperationHandle<IList<CharacterTextureAsset>> _operationHandleTexture = new AsyncOperationHandle<IList<CharacterTextureAsset>>();
        private string currentElementType;
        private string _currentSelectType;

        protected override async void OnEnable()
        {
            base.OnEnable();
            _character.creatorState = CreatorState.Enable;
            await OnLoadCharacter();
            m_CharacterInputText.text = GM.characterInstance.name;
            
            SelectElement(m_defaultToggleTab);
        }

        protected override void Start()
        {
            base.Start();
            
            _renderCharacter ??= GetComponent<RenderCharacter>();
        }

        protected override void Update()
        {
            base.Update();

            if(_instance != null && m_CharacterText != null)
                m_CharacterText.text = $"SaveName : {_instance.name}";
        }

        public void SelectElement(string elementType)
        {
            if(elementType == currentElementType)
                return;
            
            //Assets Filter
            GM.characterInstance.gender = GM.gender;
            currentElementType = elementType;
            List<string> assetFilter = GetNameFilter(elementType);
            
            Debug.Log($"Element Type = {elementType}");
            ClearChild();
            
            GetElementDataAssets(elementType, assetFilter);
        }

        private void ClearChild()
        {
            foreach (Transform child in m_contentScrollView.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private List<string> GetNameFilter(string type)
        {
            //Gender Filter
            List<string> filters = new List<string>();
            string genderFilter = ignoreGender ? "" : GM.characterInstance.gender;

            //Ethnic Culture Filter
            /*
            string ethnicCultureFilter = "";
            ThaiEthnicCulture thaiEthnicCulture = (ThaiEthnicCulture)GM.characterInstance.ethnicCulture;
            if(thaiEthnicCulture != ThaiEthnicCulture.Indeterminate)
                ethnicCultureFilter = "," + Enum.GetName(typeof(ThaiEthnicCulture), (ThaiEthnicCulture)GM.characterInstance.ethnicCulture);
                */

            //Assets Filter
            if(!String.IsNullOrEmpty(genderFilter))
                filters.Add(genderFilter); // Gender Type
            
            filters.Add(type); // Clothes Type
            
            if (type == "Skin" || type == "Face")
            {
                if(!String.IsNullOrEmpty(m_theme))
                    filters.Add(m_theme); // Theme Type
                
                return filters;
            }
            else
            {
                if(!String.IsNullOrEmpty(m_clothes))
                    filters.Add(m_clothes); // List Type
                
                return filters;
            }
        }
        
        /// <summary>
        /// filtersLabel = "filtersLabelA,filtersLabelB,filtersLabelC,filtersLabel..."
        /// </summary>
        /// <param name="filtersLabel">Filters Label list</param>
        private async void GetElementDataAssets(string type, List<string> filtersLabel)
        {
            //Show Loading
            m_loading.SetActive(true);
            string _instanceType = type;
            _currentSelectType = type;
            
            await AM.LoadMultiAssetsWithLabels(filtersLabel.ToList(), Addressables.MergeMode.Intersection, (assets, operationHandleGameObject, operationHandleMaterial, operationHandleTexture) =>
            {
                string _elementTypeRun = _instanceType;
                
                if(_elementTypeRun != _currentSelectType)
                    return;
                
                List<object> meshSort = assets[0].OrderBy(a => ((CharacterAsset)a).clothing).ToList();
                List<object> matSort = assets[1].OrderBy(a => ((CharacterAsset)a).clothing).ToList();
                List<object> textureSort = assets[2].OrderBy(a => ((CharacterAsset)a).clothing).ToList();
                
                assets[0] = meshSort;
                assets[1] = matSort;
                assets[2] = textureSort;
                
                /*foreach (var aaaasass in assets[3])
                {
                    Debug.Log($"A Num {((CharacterAsset)aaaasass).name} || {((CharacterAsset)aaaasass).type.ToString()}");
                }*/
                
                for (int i = 0; i < assets.Count; i++)
                {
                    CreateButton(type, assets[i]);
                    
                    if(((assets[1].Count > 0) || (assets[2].Count > 0) && i == 0)|| (assets[i].Count <= 0 && i > 0))
                        continue;
                    
                    for (int j = 0; j < 3 - assets[i].Count; j++)
                    {
                        Instantiate(m_contentElement, m_contentScrollView.transform);
                    }
                }
                
                _operationHandleGameObject = operationHandleGameObject;
                _operationHandleMaterial = operationHandleMaterial;
                _operationHandleTexture = operationHandleTexture;
            });
            
            //Hide Loading
            m_loading.SetActive(false);
        }

        private void CreateButton(string type, List<object> assets, bool isNull = false)
        {
            for (int i = 0; i < assets.Count; i++)
            {
                //print($"Assets is : {((CharacterAsset)assets[i]).name}");
                
                //Get Ethnic Culture Type From Assets Name
                CharacterAsset asset = (CharacterAsset)assets[i];
                string elementType = type;
                int typeIndex = i;
                bool isEquip = false;
                string[] names = asset.name.Split("_");
                string ethnicCulture = "";
                if (names.Length >= 2)
                {
                    ethnicCulture = names[0];
                }
                else
                {
                    ethnicCulture = "Indeterminate";
                }

                //Create Button
                GameObject buttonObject = Instantiate(m_contentElement, m_contentScrollView.transform);
                
                //If Check Button Null Go Return
                if(isNull)
                    return;
                
                //Set Button Action
                SetButton(buttonObject, asset,
                    () =>
                    {
                        /*print($"Old Name : {oldAssetName} : New Name : {asset.name}");*/
                        
                        //Check Equip When Old Name == Current Name
                        bool hasValue = GM.characterInstance.characterWardrobe.TryGetValue(names.Length > 2 ? $"{names[1]}_{names[2]}" : names[1], out string outName);
                        isEquip = hasValue && outName == asset.name;
                        if (!isEquip)
                        {
                            /*if (names.Length > 2)
                            {
                                if (names[2] == "HeadItem" && !CheckHeadItem(names[0]))
                                {
                                    print($"{names[2]} : Not Match !!!!!!!!");
                                    return;
                                }
                            }*/
                            
                            //Set Assets to Character
                            _character.SetCharacterElement(type, asset, asset.name);
                            
                            //Convert ThaiEthnicCulture String To Enum
                            //ThaiEthnicCulture thaiEthnicCulture = (ThaiEthnicCulture)Enum.Parse(typeof(ThaiEthnicCulture), ethnicCulture);
                            print($"Enum = {ethnicCulture}");
                            //print($"--------{thaiEthnicCulture}---------");
                        }
                        else
                        {
                            //Convert ThaiEthnicCulture String To Enum
                            //ThaiEthnicCulture thaiEthnicCulture = (ThaiEthnicCulture)Enum.Parse(typeof(ThaiEthnicCulture), ethnicCulture);
                            
                            //UnEquip And Remove Clothes To Default

                            string[] types = type.Split(",");
                            if (types.Length <= 1)
                            {
                                RemoveClothes(names.Length <= 2 ? elementType : $"{elementType}_{names[2]}");
                            }
                            else
                            {
                                string[] assetsNames = names;
                                string elementTypeFile = assetsNames.Length <= 2 ? elementType : 
                                    (assetsNames.Length > 3 
                                        ? $"{elementType}_{assetsNames[2]}_{assetsNames[3]}" : 
                                        $"{elementType}_{assetsNames[2]}");
                                
                                RemoveClothes(elementTypeFile);
                            }
                        }
                    });
            }
        }

        private String GetAssetType(string[] name)
        {
            if (name.Length <= 1)
            {
                return name[0];
            }
            else
            {
                return name.Length > 2 ? name[2] : name[1];
            }
        }
        
        //Set Button Name And Icon
        private void SetButton(GameObject buttonObject, CharacterAsset asset, UnityAction buttonAction)
        {
            buttonObject.name = asset.name;
            CanvasComponentList _canvasComponentList = buttonObject.GetComponent<CanvasComponentList>();
            _canvasComponentList.texts[0].text = asset.name;
            _canvasComponentList.image[0].sprite = asset.icon;
            Button button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(buttonAction);
        }

        public override void OnSaveCharacter()
        {
            _renderCharacter.RenderAndSave(GM.renderCharacterSize);
            
            base.OnSaveCharacter();
        }

        public void ResetCurrentSelectElement()
        {
            currentElementType = "";
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _character.creatorState = CreatorState.Disable;
        }
    }
}