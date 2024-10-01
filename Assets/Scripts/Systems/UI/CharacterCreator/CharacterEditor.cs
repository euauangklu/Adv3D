using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace GDD
{
    public class CharacterEditor : MonoBehaviour, IBackButton, IComponentUI
    {
        [ToggleGroup("ShowOtherSettings")] public bool ShowOtherSettings;
        [Header("Add Event")] 
        [ToggleGroup("ShowOtherSettings")][SerializeField] protected UnityEvent m_addEventCloseUI;
        [ToggleGroup("ShowOtherSettings")][SerializeField] protected UnityEvent m_onDelete;
        [ToggleGroup("ShowOtherSettings")][SerializeField] protected UnityEvent m_onSave;
        [ToggleGroup("ShowOtherSettings")][SerializeField] private Transform m_parant;
        
        [Header("OnEnable Event")] 
        [ToggleGroup("ShowOtherSettings")][SerializeField] protected UnityEvent onEnableUI;
        
        [Header("OnDisable Event")] 
        [ToggleGroup("ShowOtherSettings")][SerializeField] protected UnityEvent onDisableUI;
        
        [Header("Loading")]
        [ToggleGroup("ShowOtherSettings")][SerializeField] protected GameObject m_loading;
        
        [Header("Character Gender")]
        [ToggleGroup("ShowOtherSettings")][SerializeField] protected string m_gender = "Male";
        
        protected GameManager GM;
        protected SaveManager SM;
        protected MassageManager MM;
        protected AddressablesManager AM;
        protected CrashesDataManager CDM;
        protected CharacterCreatorScript m_character;
        protected CharacterInstance _instance;
        protected List<AsyncOperationHandle<CharacterMeshAsset>> _operationHandleGameObject = new List<AsyncOperationHandle<CharacterMeshAsset>>();
        protected List<AsyncOperationHandle<CharacterMaterialAsset>> _operationHandleMaterial = new List<AsyncOperationHandle<CharacterMaterialAsset>>();
        protected List<AsyncOperationHandle<CharacterTextureAsset>> _operationHandleTexture = new List<AsyncOperationHandle<CharacterTextureAsset>>();
        //protected ThaiEthnicCulture _thaiEthnicCulture;
        protected GameObject m_messageHighlight;

        protected CharacterCreatorScript _character
        {
            get
            {
                GM ??= GameManager.Instance;
                GameObject characterObject = GM.TryGetCharacter(GM.characterInstance.gender, out bool hasCharacter);
                m_character ??= hasCharacter ? characterObject.GetComponent<CharacterCreatorScript>() : null;

                return m_character;
            }
        }

        protected virtual void OnEnable()
        {
            GM = GameManager.Instance;
            SM = SaveManager.Instance;
            MM = MassageManager.Instance;
            AM = AddressablesManager.Instance;
            CDM = CrashesDataManager.Instance;
            
            
            _instance = GM.characterInstance;

            GameObject character = GM.TryGetCharacter(m_gender, out bool hasCharacter);
            if(hasCharacter)
                m_character = character.GetComponent<CharacterCreatorScript>();
            
            print($"GM is null : {GM == null} || SM is null : {SM == null}");
            onEnableUI?.Invoke();
        }

        public void DebugText(string text)
        {
            print(text);
        }
        
        protected virtual void Start()
        {
            
        }

        protected virtual void Update()
        {
            
        }

        protected void ShowLoading()
        {
            //Show loading
            if (m_loading != null)
                m_loading.SetActive(true);
        }

        protected void HideLoading()
        {
            //Hide Loading
            if (m_loading != null)
                m_loading.SetActive(false);
        }
        
        protected virtual async Task OnLoadCharacter()
        {
            if (_character == null)
            {
                print("Char Null");
                return;
            }

            _instance = GM.characterInstance;
            SwitchGender();
            
            //Show loading
            ShowLoading();

            //Hide Character
            _character.gameObject.SetActive(false);

            await OnLoadDefaultCharacterAsync();
            await OnLoadCharacterAsync();

            //Show Character
            _character.gameObject.SetActive(true);

            //Hide Loading
            HideLoading();
        }

        protected async void LoadDefaultCharacter()
        {
            //Show loading
            ShowLoading();

            /*Task.WhenAll(
                //Load ItemL
                AutoLoadAssetType(_character.characterElements[ElementType.Accessories_ItemL]._settingComponent, _character.defaultOutfit[8], ElementType.Accessories),
                         
                //Load ItemR
                AutoLoadAssetType(_character.characterElements[ElementType.Accessories_ItemR]._settingComponent, _character.defaultOutfit[11], ElementType.Accessories)
            );*/
            
            //Hide Character
            _character.gameObject.SetActive(false);
            
            //SetDefault
            await Task.Run(() =>
            {
                for (int i = 0; i < GM.characterInstance.characterWardrobe.Keys.Count; i++)
                {
                    string key = GM.characterInstance.characterWardrobe.Keys.ElementAt(i);
                    GM.characterInstance.characterWardrobe[key] = _character.characterElements[key]._defaultOutfit;
                }
            });
            
            await OnLoadDefaultCharacterAsync();
            
            //Show Character
            _instance = GM.characterInstance;
            SwitchGender();
            
            //Reset Foot Offset
            _character.ResetFootOffset();
            
            /*Task.WhenAll(
                //Load ItemL
                AutoLoadAssetType(_character.characterElements[ElementType.Accessories_ItemL]._settingComponent, _character.defaultOutfit[8], ElementType.Accessories),
                         
                //Load ItemR
                AutoLoadAssetType(_character.characterElements[ElementType.Accessories_ItemR]._settingComponent, _character.defaultOutfit[11], ElementType.Accessories)
            );*/
                        
            //Hide Loading
            HideLoading();
        }
        
        private async Task OnLoadDefaultCharacterAsync()
        {
            for (int i = 0; i <  _character.characterElements.Keys.Count; i++)
            {
                var characterElements = _character.characterElements.ElementAt(i);

                if (String.IsNullOrEmpty(characterElements.Value._defaultOutfit))
                {
                    _character.SetElementType(characterElements.Value, null, characterElements.Value._defaultOutfit);
                }
                else
                {
                    await LoadCharacterComponent(characterElements.Value._settingComponent, assetResult =>
                    {
                        _character.SetElementType(characterElements.Value, assetResult, characterElements.Value._defaultOutfit);
                    }, characterElements.Value._defaultOutfit);
                }
            }
        }

        private async Task OnLoadCharacterAsync()
        {
            for (int i = 0; i < _instance.characterWardrobe.Keys.Count; i++)
            {
                var organCharacter = _instance.characterWardrobe.ElementAt(i);
                await AutoLoadAssetType(_character.characterElements[organCharacter.Key]._settingComponent, organCharacter.Value, organCharacter.Key.Split("_")[0]);
            }
        }

        protected async void RemoveClothes(string type)
        {
            GM.characterInstance.characterWardrobe[type] = _character.characterElements[type]._defaultOutfit;
            await AutoLoadAssetType(_character.characterElements[type]._settingComponent, _character.characterElements[type]._defaultOutfit, type);
        }
        
        protected void SwitchGender()
        {
            //Switch Gender
            GM.SwitchGender(GM.characterInstance.gender, charGender =>
            {
                charGender.transform.eulerAngles = new Vector3(0, 180, 0);
            });
        }

        protected async Task AutoLoadAssetType(SettingComponent component, string assetKey, string type)
        {
            print($"Asset is : {assetKey}");
            string assetKeyCopy = assetKey;
            string nullAsset = "";

            if (!string.IsNullOrEmpty(assetKeyCopy))
                nullAsset = assetKeyCopy[0].ToString();
            
            if (!string.IsNullOrEmpty(assetKey) && nullAsset != "*")
            {
                print($"Asset not null : {assetKey}");
                await LoadCharacterComponent(component, assetsResult =>
                {
                    _character.SetCharacterElement(type, assetsResult, assetKey);
                }, assetKey);
            }
            else
            {
                if (nullAsset == "*")
                {
                    print("AAAAAA :: " + assetKey);
                    string assetKeyNumber = assetKey.Substring(1);
                    _character.SetCharacterElement(type, null, assetKeyNumber);
                }
                else
                    _character.SetCharacterElement(type, null, assetKey);
            }
        }

        protected async Task LoadCharacterComponent(SettingComponent component, UnityAction<CharacterAsset> completedAction, string assetKey)
        {
            switch (component)
                {
                    case SettingComponent.MeshFilter:
                        await AM.LoadSingleAssetsWithLabels<CharacterMeshAsset>(assetKey,
                            completed => { completedAction?.Invoke(completed.Result); },
                            operationHandle => { _operationHandleGameObject.Add(operationHandle); });
                        break;
                    case SettingComponent.SkinMeshRenderer:
                        await AM.LoadSingleAssetsWithLabels<CharacterMeshAsset>(assetKey,
                            completed => { completedAction?.Invoke(completed.Result); },
                            operationHandle => { _operationHandleGameObject.Add(operationHandle); });
                        break;
                    case SettingComponent.MeshFilterControlRig:
                        await AM.LoadSingleAssetsWithLabels<CharacterMeshAsset>(assetKey,
                            completed => { completedAction?.Invoke(completed.Result); },
                            operationHandle => { _operationHandleGameObject.Add(operationHandle); });
                        break;
                    case SettingComponent.MaterialMesh:
                        await AM.LoadSingleAssetsWithLabels<CharacterMaterialAsset>(assetKey,
                            completed => { completedAction?.Invoke(completed.Result); },
                            operationHandle => { _operationHandleMaterial.Add(operationHandle); });
                        break;
                    case SettingComponent.MaterialSkinMesh:
                        await AM.LoadSingleAssetsWithLabels<CharacterMaterialAsset>(assetKey,
                            completed => { completedAction?.Invoke(completed.Result); },
                            operationHandle => { _operationHandleMaterial.Add(operationHandle); });
                        break;
                    case SettingComponent.Texture:
                        await AM.LoadSingleAssetsWithLabels<CharacterTextureAsset>(assetKey,
                            completed => { completedAction?.Invoke(completed.Result); },
                            operationHandle => { _operationHandleTexture.Add(operationHandle); });
                        break;
                    case SettingComponent.Color:
                        await AM.LoadSingleAssetsWithLabels<CharacterTextureAsset>(assetKey,
                            completed => { completedAction?.Invoke(completed.Result); },
                            operationHandle => { _operationHandleTexture.Add(operationHandle); });
                        break;
                }
        }

        public void AddEvent(UnityAction action)
        {
            m_addEventCloseUI.AddListener(action);
        }

        public virtual void OnSaveCharacter()
        {
            if (GM == null)
                GM = GameManager.Instance;
            if (SM == null)
                SM = SaveManager.Instance;
            
            if(MM != null && MM.isValid) 
                m_messageHighlight = MM.CreateMassage(
                () => { },
                () =>
                {
                    SM.SaveGameObjectData<CharacterInstance>(GM.characterInstance, GM.defaultSavePath + $"/Slot {GM.characterSlot + 1}-{GM.saveFileName}");
                    m_addEventCloseUI?.Invoke();
                    m_onSave?.Invoke();
                }, 
                "ท่านพอใจกับชุดในตอนนี้ แล้วหรือไม่",
                "ท่านสามารถเปลี่ยนแปลงชุดภายหลังได้",
                GetSortingOrder() + 2);
            else 
            {
                SM.SaveGameObjectData<CharacterInstance>(GM.characterInstance, GM.defaultSavePath + $"/Slot {GM.characterSlot + 1}-{GM.saveFileName}");
                m_addEventCloseUI?.Invoke();
                m_onSave?.Invoke();
            }
        }

        public virtual void OnDeleteCharacter()
        {
            if (GM == null)
                GM = GameManager.Instance;
            if (SM == null)
                SM = SaveManager.Instance;

            if (MM != null && MM.isValid)
            {
                MM.CreateMassage(
                    () => { },
                    () => { DeleteCharacter(); },
                    "ท่านต้องการกลับไปยังหน้า บันทึกตัวละครหรือไม่",
                    "บันทึกนี้จะถูกลบถาวร",
                    GetSortingOrder() + 2);
            }
            else
            {
                DeleteCharacter();
            }
        }

        protected void DeleteCharacter()
        {
            print($"On Delete Character");
            SM.DeleteSave(GM.defaultSavePath + $"/Slot {GM.characterSlot + 1}-{GM.saveFileName}");
            GM.ResetCharacterInstance();
            m_addEventCloseUI?.Invoke();
            m_onDelete?.Invoke();
            GM.characterInstance.gender = m_gender;
            _instance = GM.characterInstance;
            SwitchGender();
            LoadDefaultCharacter();
        }

        protected int GetSortingOrder()
        {
            if (m_parant == null)
                return GetComponent<Canvas>().sortingOrder;
            else
                return m_parant.GetComponent<Canvas>().sortingOrder;
        }
        
        public virtual void OnHighlight()
        {
            
        }

        public virtual void OnDisableHighlight()
        {
            
        }
        
        public virtual void OnHighlight(int i)
        {
            
        }

        public virtual void OnDisableHighlight(int i)
        {
            
        }

        public void ReleaseAssets()
        {
            foreach (var operationHandle in _operationHandleGameObject)
            {
                Addressables.Release(operationHandle);
            }

            foreach (var operationHandle in _operationHandleMaterial)
            {
                Addressables.Release(operationHandle);
            }
            foreach (var operationHandle in _operationHandleTexture)
            {
                Addressables.Release(operationHandle);
            }
        }

        protected virtual void OnDisable()
        {
            onDisableUI?.Invoke();
        }
    }
}