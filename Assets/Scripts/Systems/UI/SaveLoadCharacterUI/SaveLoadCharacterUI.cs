using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GDD
{
    public class SaveLoadCharacterUI : CharacterEditor
    {
        [Header("UI Elements")] 
        [SerializeField] protected GameObject m_contentScrollView;
        [SerializeField] protected GameObject m_horizonElement;
        [SerializeField] protected GameObject m_contentElement;

        [Header("Load Character Button Event")]
        [SerializeField] protected UnityEvent onShowInfo;
        [SerializeField] protected UnityEvent onLoadSave;
        [SerializeField] protected UnityEvent onSave;
        [SerializeField] protected UnityEvent m_endLoaded;

        [Header("UI")] [Tooltip("Lock Button For Tutorials")]
        [SerializeField] protected bool m_isLockButton;

        protected string fileName;
        protected List<GameObject> _verticalGroup = new List<GameObject>();
        protected List<GameObject> saveSlots = new List<GameObject>();

        protected CanvasComponentList m_highlight;

        public bool isLockButton
        {
            get => m_isLockButton;
            set => m_isLockButton = value;
        }

        protected override async void OnEnable()
        {
            base.OnEnable();
            
            //Show loading
            ShowLoading();
            
            await OnInitialized();
            
            //Hide Loading
            HideLoading();
        }

        protected virtual async Task OnInitialized()
        {
            foreach (var vGameObject in _verticalGroup)
            {
                Destroy(vGameObject);
            }
            _verticalGroup = new List<GameObject>();
            saveSlots = new List<GameObject>();
            await OnInitializeSaveFile();
            
            OnAwaitEnable();
        }

        protected virtual void OnAwaitEnable()
        {
            
        }

        protected override void Start()
        {
             base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected virtual async Task<List<FileInfo>> OnInitializeSaveFile()
        {
            var fileDatas = await SM.GetAllFileSavesAsync(GM.defaultSavePath);
            for (int i = 0; i < fileDatas.Count; i++)
            {
                CreateSaveSlot(fileDatas[i].Name, fileDatas[i].Name,i);
            }

            m_endLoaded?.Invoke();
            return fileDatas;
        }

        public virtual void OnChangedFileName(string name)
        {
            fileName = name;
        }
        
        public virtual void OnCreateSaveFile(int slot = default)
        {
            if(fileName == "")
                return;
            
            GM.ResetCharacterInstance();
            GM.SwitchGender(m_gender);
            
            SM.SaveGameObjectData<CharacterInstance>(GM.characterInstance, GM.defaultSavePath + $"/{fileName}");
            CreateSaveSlot(fileName, fileName, default,true);
        }

        public virtual async Task OnLoadSaveFile(string saveFileName)
        {   
            string path;
            if(String.IsNullOrEmpty(saveFileName))
                path = GM.defaultSavePath + $"/{GM.saveFileName}";
            else 
                path = GM.defaultSavePath + $"/{saveFileName}";

            if (File.Exists(path + ".json"))
            {
                //Show loading
                ShowLoading();
                
                await Task.Run(() => { GM.characterInstance = SM.LoadGameObjectData<CharacterInstance>(path); });
                
                //Hide Loading
                HideLoading();
            }
            else
            {
                Debug.LogError($"File not exists : ({path + ".json"})");
                return;
            }

            //Show loading
            ShowLoading();
            
            await OnLoadCharacter();
            
            //Hide Loading
            HideLoading();
        }

        public override void OnHighlight()
        {
            m_highlight.canvas_gameObjects[2].SetActive(true);
            m_highlight.canvas_gameObjects[2].GetComponent<Animator>().SetTrigger("Play");
        }

        public override void OnDisableHighlight()
        {
            m_highlight.canvas_gameObjects[2].GetComponent<Animator>().SetBool("IsLoop", false);
        }
        
        public override void OnHighlight(int i)
        {
            m_highlight.canvas_gameObjects[i].SetActive(true);
            m_highlight.canvas_gameObjects[i].GetComponent<Animator>().SetTrigger("Play");
        }

        public override void OnDisableHighlight(int i)
        {
            m_highlight.canvas_gameObjects[i].GetComponent<Animator>().SetBool("IsLoop", false);
        }

        protected virtual async Task<GameObject> CreateSaveSlot(CharacterInstance characterInstance, string name, string fileName, int slot = default, bool isLoadSave = false, bool isFirstChild = false, bool isAutoSetGroup = false)
        {
            return CreateSaveSlot(name, fileName, slot, isLoadSave, isFirstChild, isAutoSetGroup);
        }
        
        protected virtual GameObject CreateSaveSlot(string name, string fileName, int slot = default)
        {
            return CreateSaveSlot(name, fileName, slot, false, false);
        }

        protected virtual GameObject CreateSaveSlot(string name, string fileName, int slot = default, bool isLoadSave = false, bool isFirstChild = false, bool isAutoSetGroup = false)
        {
            string charName = name;

            if (saveSlots.Count % 2 == 0 && !isAutoSetGroup)
                CreateVerticalGroup();

            Transform parent_Hor = _verticalGroup[(!isAutoSetGroup ? _verticalGroup.Count - 1 : GetSlotGroup(slot))].transform;
            GameObject saveSlot = Instantiate(m_contentElement);
            saveSlots.Add(saveSlot);
            saveSlot.GetComponent<RectTransform>().SetParent(_verticalGroup[(!isAutoSetGroup ? _verticalGroup.Count - 1 : GetSlotGroup(slot))].transform);
            saveSlot.GetComponent<RectTransform>().SetParent(parent_Hor);
            saveSlot.name = fileName;
            saveSlot.transform.localScale = Vector3.one;

            if(isFirstChild)
                saveSlot.transform.SetSiblingIndex(0);
            
            RectTransform saveSlotRect = saveSlot.GetComponent<RectTransform>();
            saveSlotRect.localPosition = Vector3.zero;
            saveSlotRect.localScale = Vector3.one;
            
            CanvasComponentList componentList = saveSlot.GetComponent<CanvasComponentList>();
            componentList.texts[0].text = charName;
            componentList.texts[1].text = charName;
            componentList.canvas_gameObjects[1].SetActive(charName == GM.characterInstance.name);
            componentList.image[3].enabled = fileName == GM.fileName;

            if (slot == 0)
            {
                m_highlight = componentList;
            }

            SetButtonEvent(componentList, saveSlot, charName, slot);
            
            return saveSlot;
        }

        protected int GetSlotGroup(int slot)
        {
            float value = slot * 0.5f;
            Debug.LogWarning($"Group Slot : {value}");
            return Mathf.FloorToInt(value);
        }
        
        protected Transform CreateVerticalGroup()
        {
            GameObject verticalGroup = Instantiate(m_horizonElement, m_contentScrollView.transform);
            verticalGroup.transform.localPosition = Vector3.zero;
            
            _verticalGroup.Add(verticalGroup);
            return verticalGroup.transform;
        }

        protected virtual void SetButtonEvent(CanvasComponentList componentList, GameObject saveSlot, string charName, int slot = default)
        {
            print("Base!!!!!!!");
            componentList.buttons[0].onClick.AddListener(() =>
            {
                LoadSaveSlot(saveSlot);
                onLoadSave?.Invoke();
            });
            componentList.buttons[1].onClick.AddListener(() =>
            {
                RemoveSaveSlot(saveSlot);
                onSave?.Invoke();
            });
        }

        protected virtual async void LoadSaveSlot(GameObject saveSlot, int slot = default)
        {
            //Show loading
            ShowLoading();
            
            string nameSlot = saveSlot.name;
            await Task.Run(() =>
            {
                GM.characterInstance =
                    SM.LoadGameObjectData<CharacterInstance>(GM.defaultSavePath + $"/{nameSlot}");
            });
            await OnLoadCharacter();
            GM.saveFileName = saveSlot.name;
            
            //Hide Loading
            HideLoading();
        }
        
        protected virtual void RemoveSaveSlot(GameObject saveSlot)
        {
            RemoveSaveSlot(saveSlot);
        }
        
        protected virtual void RemoveSaveSlot(GameObject saveSlot, string name = default, int slot = default)
        {
            if(name == "")
                SM.DeleteSave(GM.defaultSavePath + $"/{saveSlot.name}");
            else
                SM.DeleteSave(GM.defaultSavePath + $"/{name}");
            
            Destroy(saveSlot);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }
}