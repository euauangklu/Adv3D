using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using SolidUtilities.Collections;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    [RequireComponent(typeof(RenderCharacter))]
    public class SaveLoadCharacterLimitSlotUI : SaveLoadCharacterUI
    {
        [Header("Save Slot")]
        [SerializeField]
        protected int m_saveSlot;
        [SerializeField]
        protected TextMeshProUGUI m_saveSlotText;
        
        [SerializeField]
        private SerializableDictionary<ThaiEthnicCulture ,Sprite> m_maleClutureIcon;
        
        [SerializeField]
        private SerializableDictionary<ThaiEthnicCulture ,Sprite> m_femaleClutureIcon;

        [SerializeField] private RenderCharacter _renderCharacter;
        
        protected int _currentNumberSlot;
        protected Dictionary<int, Tuple<GameObject, bool>> _buttons = new ();

        public int currentNumberSlot
        {
            get => _currentNumberSlot;
        }

        public int maxSaveSlot
        {
            get => m_saveSlot;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override async Task OnInitialized()
        {
            if(_buttons.Count > 0)
                ClearButton();
            
            await base.OnInitialized();
        }

        protected override void OnAwaitEnable()
        {
            base.OnAwaitEnable();
            
            m_saveSlotText.text = $"{_currentNumberSlot} / {m_saveSlot}";
        }

        protected override void Start()
        {
            base.Start();

            _renderCharacter ??= GetComponent<RenderCharacter>();
        }
        
        protected virtual void ClearButton()
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                Destroy(_buttons[i].Item1);
            }

            _buttons = new Dictionary<int, Tuple<GameObject, bool>>();
        }

        protected override async Task<List<FileInfo>> OnInitializeSaveFile()
        {
            var fileDatas = await SM.GetAllFileSavesAsync(GM.defaultSavePath);
            FileInfo[] fileDatasClone = new FileInfo[fileDatas.Count];
            Array.Copy(fileDatas.ToArray(), fileDatasClone, fileDatas.Count);
            var filedatasRemove = fileDatasClone.ToList();
            
            _currentNumberSlot = fileDatas.Count;

            for (int i = 0; i < m_saveSlot; i++)
            {
                if (filedatasRemove.Count > 0)
                {
                    bool isFound = false;
                    string[] fileSaveName = new string[2];
                    FileInfo removeSaveAT = null;
                    for (int j = 0; j < filedatasRemove.Count; j++)
                    {
                        fileSaveName = filedatasRemove[j].Name.Split(".")[0].Split("-");
                        
                        if (fileSaveName[0] == $"Slot {i + 1}")
                        {
                            isFound = true;
                            removeSaveAT = filedatasRemove[j];
                            break;
                        }
                    }

                    if (isFound)
                    {
                        StreamReader reader = removeSaveAT.OpenText();
                        string jsonFile = reader.ReadToEnd();
                        CharacterInstance saveCI = JsonConvert.DeserializeObject<CharacterInstance>(jsonFile);
                        await CreateSaveSlot(saveCI, saveCI.name, fileSaveName[1], i, true, false, false);
                        filedatasRemove.Remove(removeSaveAT);
                        
                        reader.Close();
                    }
                    else
                    {
                        await CreateSaveSlot(null, $"SaveSlot {i + 1}", $"SaveSlot {i + 1}", i, false, false, false);
                    }
                }
                else
                {
                    await CreateSaveSlot(null, $"SaveSlot {i + 1}", $"SaveSlot {i + 1}", i, false, false, false);
                }
            }

            if (m_saveSlot % 2 >= 1)
            {
                Transform parent_Hor;
                if (saveSlots.Count % 2 == 0)
                    parent_Hor = CreateVerticalGroup();
                else 
                    parent_Hor = _verticalGroup[_verticalGroup.Count - 1].transform;
                
                CreateBlankButton(parent_Hor);
                parent_Hor.localScale = Vector3.one;
                parent_Hor.localPosition = Vector3.zero;
            }

            m_endLoaded?.Invoke();
            return fileDatas;
        }

        public override async Task OnLoadSaveFile(string saveFileName)
        {
            if(String.IsNullOrEmpty(saveFileName))
                await base.OnLoadSaveFile($"Slot {GM.characterSlot + 1}-{GM.saveFileName}");
            else
                await base.OnLoadSaveFile(saveFileName);
        }

        protected async override Task<GameObject> CreateSaveSlot(CharacterInstance characterInstance, string name, string fileName, int slot = default, bool isLoadSave = false, bool isFirstChild = false, bool isAutoSetGroup = false)
        {
            GameObject button = base.CreateSaveSlot(name, fileName, slot, isLoadSave, isFirstChild, isAutoSetGroup);
            Tuple<GameObject, bool> buttonData = new Tuple<GameObject, bool>(button, isLoadSave);
            CanvasComponentList _canvaslist = button.GetComponent<CanvasComponentList>();

            if (characterInstance != null)
            {
                //ThaiEthnicCulture ethnicCulture;

                if (!String.IsNullOrEmpty(characterInstance.theme))
                {
                    Image image = _canvaslist.image[2];
                    Texture2D _texture2D = await _renderCharacter.LoadImageFromSave(fileName, GM.renderCharacterSize);
                    Rect rect = new Rect(0, 0, GM.renderCharacterSize.x, GM.renderCharacterSize.y);
                    
                    if(_texture2D != null)
                        image.sprite = Sprite.Create(_texture2D, rect, Vector2.zero);
                    else
                    {
                        Debug.LogWarning($"Missing character image : {fileName}");
                    }

                    /*ThaiEthnicCulture.TryParse(characterInstance.theme, out ethnicCulture);

                    if (characterInstance.gender == m_gender)
                        _canvaslist.image[2].sprite = m_maleClutureIcon[ethnicCulture];
                    else
                        _canvaslist.image[2].sprite = m_femaleClutureIcon[ethnicCulture];*/
                }
            }
            
            if (isLoadSave)
            {
                _canvaslist.canvas_gameObjects[0].SetActive(true);
                _canvaslist.image[1].gameObject.SetActive(false);
            }
            else
            {
                _canvaslist.canvas_gameObjects[0].SetActive(false);
                _canvaslist.image[1].gameObject.SetActive(true);
            }

            if (_buttons.TryGetValue(slot, out var outButton))
                _buttons[slot] = buttonData;
            else
                _buttons.Add(slot, buttonData);
            
            return button;
        }
        
        private void CreateBlankButton(Transform parent)
        {
            GameObject saveSlotEE = Instantiate(m_contentElement);
            saveSlotEE.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            saveSlotEE.GetComponent<Button>().enabled = false;
            saveSlotEE.transform.GetChild(0).gameObject.SetActive(false);
            saveSlotEE.transform.SetParent(parent);
            saveSlotEE.transform.localScale = Vector3.one;
        }

        protected override void SetButtonEvent(CanvasComponentList componentList, GameObject saveSlot, string charName, int slot = default)
        {
            if (slot > 0 && m_isLockButton)
                return;
            
            componentList.buttons[0].onClick.AddListener((() =>
            {
                GM.characterSlot = slot;
                
                if (_buttons[slot].Item2)
                {
                    LoadSaveSlot(saveSlot, slot);
                    onLoadSave?.Invoke();
                    Debug.LogWarning("Load Save");
                }
                else
                {
                    OnCreateSaveFile(slot);
                    Debug.LogWarning("Create New Save");
                    onSave?.Invoke();
                }
            }));
            componentList.buttons[1].onClick.AddListener(() =>
            {
                RemoveSaveSlot(saveSlot, $"/Slot {slot + 1}-{saveSlot.name}", slot);
                Debug.LogWarning("Delete Save");
            });
            componentList.buttons[2].onClick.AddListener(() =>
            {
                GM.characterSlot = slot;
                LoadSaveSlot(saveSlot, slot);
                onShowInfo?.Invoke();
                Debug.LogWarning("Edit Save");
            });
        }

        public override void OnCreateSaveFile(int slot = default)
        {
            GM.ResetCharacterInstance();
            LoadDefaultCharacter();
            
            SM.SaveGameObjectData<CharacterInstance>(GM.characterInstance, GM.defaultSavePath + $"/Slot {slot + 1}-{GM.saveFileName}");
        }

        protected override async void LoadSaveSlot(GameObject saveSlot, int slot = default)
        {
            GM.ResetCharacterInstance();
            string nameSlot = saveSlot.name;
            await Task.Run(() =>
            {
                GM.characterInstance = SM.LoadGameObjectData<CharacterInstance>(GM.defaultSavePath + $"/Slot {slot + 1}-{nameSlot}");
            });
            
            await OnLoadCharacter();
            GM.saveFileName = saveSlot.name;
        }

        protected override void RemoveSaveSlot(GameObject saveSlot, string name = default, int slot = default)
        {
            Task<GameObject> taskCSS = CreateSaveSlot(null, $"SaveSlot {slot + 1}", $"SaveSlot {slot + 1}", slot, false, false, true);
            GameObject resultGameObject = taskCSS.Result;
            resultGameObject.transform.SetSiblingIndex(slot % 2);
            
            base.RemoveSaveSlot(saveSlot, name, slot);
            _renderCharacter.RemoveImageSave(saveSlot.name);

            if (_currentNumberSlot > 0)
            {
                var fileDatas = SM.GetAllFileSaves(GM.defaultSavePath);
                _currentNumberSlot = fileDatas.Count;
                m_saveSlotText.text = $"{_currentNumberSlot} / {m_saveSlot}";
            }
        }

        protected override void OnDisable()
        {
            
        }
    }
}