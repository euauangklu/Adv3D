using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GDD
{
    public class CollectionUI : CharacterEditor
    {
        [SerializeField] private TextMeshProUGUI m_unlockCountText;
        [SerializeField] private int m_rowCount = 4;
        [SerializeField] private GameObject m_content;
        [SerializeField] private GameObject m_titlePrefab;
        [SerializeField] private GameObject m_verticalGroupPrefab;
        [SerializeField] private GameObject m_buttonClothesPrefab;
        [SerializeField] private GameObject m_showCollectionDetail;
        [SerializeField] private UnityEvent m_onShowCollection;
        [SerializeField] private UnityEvent m_onUnlockAll;

        private List<GameObject> _title = new List<GameObject>();
        private List<GameObject> _verticalGroup = new List<GameObject>();
        private List<GameObject> _buttonClothes = new List<GameObject>();
        private int _totalUnlock;
        private int _totalAssets;
        CanvasComponentList _canvasCollection;
        private Dictionary<int, List<object>> collectAssets;
        
        protected async override void OnEnable()
        {
            base.OnEnable();
            
            _canvasCollection = m_showCollectionDetail.GetComponent<CanvasComponentList>();
            
            if (_title.Count > 0)
                ClearCollection();
            
            await CDM.OnInitialization(m_loading);
            collectAssets = await CDM.GetCollectAssets();
            
            CreateCollection();
        }

        public async void PreInitialization()
        {
            AM ??= AddressablesManager.Instance;
            GM ??= GameManager.Instance;
            CDM ??= CrashesDataManager.Instance;
            await CDM.OnInitialization(m_loading, false);
            collectAssets = await CDM.GetCollectAssets();
        }
        
        

        private void ClearCollection()
        {
            //Reset Total
            _totalUnlock = 0;
            _totalAssets = 0;
            
            foreach (var ti in _title)
            {
                Destroy(ti);
            }

            foreach (var _ver in _verticalGroup)
            {
                Destroy(_ver);
            }

            foreach (var _button in _buttonClothes)
            {
                Destroy(_button);
            }
        }
        
        private void CreateCollection()
        {
            //Create Button Collection
            string[] nameEnum = Enum.GetNames(typeof(ThaiEthnicCulture));
            for (int i = 1; i < nameEnum.Length; i++)
            {
                GetAsset((ThaiEthnicCulture)i, i);
            }
            
            //Show Total Unlock
            m_unlockCountText.text = $"{_totalUnlock} /{_totalAssets}";
            if(_totalUnlock == _totalAssets)
                m_onUnlockAll?.Invoke();
        }

        private string GetNameThaiEthnicCulture(ThaiEthnicCulture thaiEthnicCulture)
        {
            switch (thaiEthnicCulture)
            {
                case ThaiEthnicCulture.TaiKuen:
                    return "ไทเขิน";
                case ThaiEthnicCulture.TaiLue:
                    return "ไทลื้อ";
                case ThaiEthnicCulture.TaiYai:
                    return "ไทใหญ่";
                case ThaiEthnicCulture.TaiYong:
                    return "ไทยอง";
                case ThaiEthnicCulture.TaiYuan :
                    return "ไทยวน";
                default:
                    return "";
            }
        }

        private void GetAsset(ThaiEthnicCulture _thaiEthnicCulture, int index)
        {
            //Create Title
            CreateTitle(GetNameThaiEthnicCulture((ThaiEthnicCulture)index));
            int assetNum = 0;
            int verNum = 0;

            List<object> _callBackAssets = new List<object>();
            
            foreach (var key in collectAssets.Keys)
            {
                _callBackAssets.AddRange(collectAssets[key]
                    .Where(a => ((CharacterAsset)a).name.Split("_")[0] == _thaiEthnicCulture.ToString()).ToList());
            }
            
            //Order Assets Unlock To Top
            _callBackAssets = _callBackAssets.OrderBy(a => ((CharacterAsset)a).isUnlock ? 0 : 1).ToList();

            //Loop Create Button
            for (int i = 0; i < _callBackAssets.Count; i++)
            {
                assetNum++;

                if (_buttonClothes.Count % m_rowCount == 0)
                {
                    verNum++;
                    CreateVerticalGroup();
                }

                CharacterAsset asset = (CharacterAsset)_callBackAssets[i];
                CreateButton(asset, _verticalGroup[_verticalGroup.Count - 1].transform, asset.assetName, GetNameThaiEthnicCulture((ThaiEthnicCulture)index));
            }

            //Loop Create Blank Button
            int _currentVerticalCount = verNum * 4;
                
            /*print($"Vertical : {_currentVerticalCount}");
            print($"Assets : {assetNum}");
            print($"Total : {_currentVerticalCount - assetNum}");*/
                
            for (int i = 0; i < _currentVerticalCount - assetNum; i++)
            {
                CreateBlankButton(_verticalGroup[_verticalGroup.Count - 1].transform);
            }
                
            //Add Total Assets
            _totalAssets += assetNum;
        }

        private void CreateTitle(string name)
        {
            GameObject titleObject = Instantiate(m_titlePrefab, m_content.transform);
            titleObject.transform.position = Vector3.zero;
            titleObject.GetComponent<CanvasComponentList>().texts[0].text = name;
            
            _title.Add(titleObject);
        }

        private void CreateVerticalGroup()
        {
            GameObject verticalGroup = Instantiate(m_verticalGroupPrefab, m_content.transform);
            verticalGroup.transform.position = Vector3.zero;
            
            _verticalGroup.Add(verticalGroup);
        }

        private void CreateButton(CharacterAsset asset, Transform parent, string elementType, string thaiEthnicCulture, int index = -1)
        {
            print($"Parebt {parent.name}");
            //Add Unlock Count
            _totalUnlock = asset.isUnlock ? _totalUnlock + 1 : _totalUnlock;
            
            //Create Button
            GameObject button = Instantiate(m_buttonClothesPrefab, parent);
            button.transform.position = Vector3.zero;
            
            if(index >= 0)
                button.transform.SetSiblingIndex(index);
            
            CanvasComponentList _canvasButton = button.GetComponent<CanvasComponentList>();
            _canvasButton.texts[0].text = asset.name;
            _canvasButton.image[0].sprite = asset.isUnlock ? asset.unlockIcon : asset.lockIcon;
            _canvasButton.canvas_gameObjects[0].SetActive(!asset.isRead && asset.isUnlock);
            
            Button _clothesButton = _canvasButton.buttons[0];
            _clothesButton.interactable = asset.isUnlock;
            _clothesButton.onClick.AddListener(() =>
            {
                asset.isRead = true;
                _canvasButton.canvas_gameObjects[0].SetActive(false);
                m_showCollectionDetail.SetActive(true);

                _canvasCollection.image[0].sprite = asset.icon;
                _canvasCollection.texts[0].text = asset.assetsDescription;
                _canvasCollection.texts[1].text = elementType;
                _canvasCollection.texts[2].text = thaiEthnicCulture;
                _canvasCollection.texts[3].text = asset.assetName + thaiEthnicCulture;
                
                m_onShowCollection?.Invoke();
            });
            
            _buttonClothes.Add(button);
        }

        private void CreateBlankButton(Transform parent)
        {
            GameObject button = Instantiate(m_buttonClothesPrefab, parent);
            button.transform.position = Vector3.zero;
            CanvasComponentList _canvasButton = button.GetComponent<CanvasComponentList>();
            _canvasButton.canvas_gameObjects[0].SetActive(false);
            _canvasButton.canvas_gameObjects[1].SetActive(false);
            _buttonClothes.Add(button);
        }
    }
}