using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class UnlockAllAssetsEvent : MonoBehaviour
    {
        [SerializeField] private SaveLoadCharacterLimitSlotUI _saveLoadCharacterUI;
        [SerializeField] private UnityEvent m_unlockAll;
        [SerializeField] private TutorialImagesList _tutorialImagesList;
        private CrashesDataManager CDM;
        private Dictionary<int, List<object>> assets;
        private bool isUnlock;

        private async void Start()
        {
            _saveLoadCharacterUI ??= GetComponent<SaveLoadCharacterLimitSlotUI>();
            CDM ??= CrashesDataManager.Instance;
            assets = await CDM.GetCollectAssets();
        }

        private void Update()
        {
            if (_saveLoadCharacterUI.currentNumberSlot >= _saveLoadCharacterUI.maxSaveSlot && !_tutorialImagesList.isShowed)
            {
                if (CheckCurrentUnlockAssets() && !isUnlock)
                {
                    m_unlockAll?.Invoke();
                    isUnlock = true;
                }
            }
            else
            {
                isUnlock = false;
            }
        }
        
        private bool CheckCurrentUnlockAssets()
        {
            List<object> _callBackAssets = new List<object>();
            foreach (var key in assets.Keys)
            {
                _callBackAssets.AddRange(assets[key]);
            }

            var all = _callBackAssets.Where(a => ((CharacterAsset)a).isUnlock).ToList();
            return all.Count >= _callBackAssets.Count;
        }
    }
}