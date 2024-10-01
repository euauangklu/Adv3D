using System.Collections.Generic;
using System.Threading.Tasks;
using GDD.Sinagleton;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GDD
{
    public class CrashesDataManager : DontDestroy_Singleton<CrashesDataManager>
    {
        private Dictionary<int, List<object>> _collectAssets = new Dictionary<int, List<object>>();
        private AddressablesManager AM;

        public override void OnAwake()
        {
            base.OnAwake();
            AM ??= AddressablesManager.Instance;
        }
        
        public async Task OnInitialization(GameObject m_loading, bool showLoading = true)
        {
            Debug.Log("OnInitialization Assets");
            
            //If Have Collect Assets//
            if(_collectAssets.Count > 0)
                return;
            
            //Show loading
            if(m_loading != null && showLoading)
                m_loading.SetActive(true);
            
            List<string> keys = new List<string>()
            {
                "Collection"
            };
            
            UnityAction<Dictionary<int, List<object>>, AsyncOperationHandle<IList<CharacterMeshAsset>>, AsyncOperationHandle<IList<CharacterMaterialAsset>>, AsyncOperationHandle<IList<CharacterTextureAsset>>> callBack;
            callBack = (_callBackAssets, handle, operationHandle, _callBack) =>
            {
                _collectAssets = _callBackAssets;
                Debug.Log($"OnInitialization Assets = {_callBackAssets.Values.Count}");
            };
            
            await AM.LoadMultiAssetsWithLabels(keys, Addressables.MergeMode.Intersection, callBack);
            
            //Hide Loading
            if(m_loading != null && showLoading)
                m_loading.SetActive(false);
        }

        public async Task<Dictionary<int, List<object>>> GetCollectAssets()
        {
            if (_collectAssets == null || _collectAssets.Count <= 0)
            {
                AM ??= AddressablesManager.Instance;
                await OnInitialization(null, false);
            }

            return _collectAssets;
        }
    }
}