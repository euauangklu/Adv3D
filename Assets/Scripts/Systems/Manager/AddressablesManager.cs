using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GDD;
using GDD.Sinagleton;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public class AddressablesManager : CanDestroy_Sinagleton<AddressablesManager>
{
    //[SerializeField] AssetReference 
    private UnityAction _OnInitialize;
    private Dictionary<int, List<object>> _crashAssets = new Dictionary<int, List<object>>();

    public UnityAction OnInitialize
    {
        get => _OnInitialize;
        set => _OnInitialize = value;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Addressables.InitializeAsync().Completed += AddressableInitializeCompleted;
        
        /*
        UnityAction<Dictionary<int, List<object>>> _LoadWithLabelsCompleted;
        _LoadWithLabelsCompleted = testtt =>
        {
            foreach (object aseet in testtt[0])
            {
                print($">>> Name : {((GameObject)aseet).name}");
            }

            foreach (object aseet in testtt[1])
            {
                print($">>> Name : {((Material)aseet).name}");
            }

            foreach (object aseet in testtt[2])
            {
                print($">>> Name : {((Texture)aseet).name}");
            }
        };*/
    }
    
    private async void AddressableInitializeCompleted(AsyncOperationHandle<IResourceLocator> OperationHandle)
    {
        if (OperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            print($"Completed");
            _OnInitialize?.Invoke();
        }
    }

    public async Task OnCrashAssets(List<string> keys)
    {
        Debug.Log("OnInitialization Assets");

        //If Have Collect Assets//
        if (_crashAssets.Count > 0)
            return;

        UnityAction<Dictionary<int, List<object>>, AsyncOperationHandle<IList<CharacterMeshAsset>>, AsyncOperationHandle<IList<CharacterMaterialAsset>>, AsyncOperationHandle<IList<CharacterTextureAsset>>> callBack;
        callBack = (_callBackAssets, handle, operationHandle, _callBack) =>
        {
            _crashAssets = _callBackAssets;
            Debug.Log($"OnInitialization Assets = {_callBackAssets.Values.Count}");
        };

        await LoadMultiAssetsWithLabels(keys, Addressables.MergeMode.Intersection, callBack);
    }

    public async Task<Dictionary<int, List<object>>> GetAssetsCrash(List<string> keys)
    {
        //If Have Collect Assets//
        if (_crashAssets.Count > 0)
            return _crashAssets;
        else
        {
            await OnCrashAssets(keys);
            return _crashAssets;
        }
    }

    /*public async Task LoadSingleAssetsWithName<T>(string name, UnityAction<AsyncOperationHandle<T>> callBack)
    {
        AddressablesManager<T> assetsLoad;

        assetsLoad = Addressables.LoadAssetAsync<>()
    }*/

    public async Task LoadSingleAssetsWithLabels<T>(string assetKey, UnityAction<AsyncOperationHandle<T>> completed, UnityAction<AsyncOperationHandle<T>> callBack)
    {
        if(assetKey == null)
            return;
            
        AsyncOperationHandle<T> assetAsync;
        print($"Name : {assetKey}");
        assetAsync = Addressables.LoadAssetAsync<T>(assetKey);
        assetAsync.Completed += handle =>
        {
            print("Completed!");
            if(handle.Status == AsyncOperationStatus.Succeeded)
                completed?.Invoke(handle);
        };

        await assetAsync.Task;
        callBack?.Invoke(assetAsync);
    }
    
    public async Task LoadMultiAssetsWithLabels(List<string> KeysLabels, Addressables.MergeMode mergeMode, UnityAction<Dictionary<int, List<object>>, AsyncOperationHandle<IList<CharacterMeshAsset>>, AsyncOperationHandle<IList<CharacterMaterialAsset>>, AsyncOperationHandle<IList<CharacterTextureAsset>>> callBack)
    {
        //Assets Data
        Dictionary<int, List<object>> callBackAssets = new Dictionary<int, List<object>>();
        AsyncOperationHandle<IList<CharacterMeshAsset>> operationHandleGameObject = new AsyncOperationHandle<IList<CharacterMeshAsset>>();
        AsyncOperationHandle<IList<CharacterMaterialAsset>> operationHandleMaterial = new AsyncOperationHandle<IList<CharacterMaterialAsset>>();
        AsyncOperationHandle<IList<CharacterTextureAsset>> operationHandleTexture = new AsyncOperationHandle<IList<CharacterTextureAsset>>();

        await Task.WhenAll(OnLoadWithLabelsOperationHandle<CharacterMeshAsset>(
            KeysLabels, mergeMode, (assets, operationHandle) =>
            {
                /*foreach (var asset in assets)
                {
                    print($">> Name : {((CharacterMeshAsset)asset).name}");
                }*/

                callBackAssets.Add(0, assets);
                operationHandleGameObject = operationHandle;
            }), OnLoadWithLabelsOperationHandle<CharacterMaterialAsset>(
            KeysLabels, mergeMode, (assets, operationHandle) =>
            { 
                /*foreach (var asset in assets)
                {
                    print($">> Name : {((CharacterMaterialAsset)asset).name}");
                }*/

                callBackAssets.Add(1, assets);
                operationHandleMaterial = operationHandle;
            }), OnLoadWithLabelsOperationHandle<CharacterTextureAsset>(
            KeysLabels, mergeMode, (assets, operationHandle) =>
            {
                /*foreach (var asset in assets)
                {
                    print($">> Name : {((CharacterTextureAsset)asset).name}");
                }*/

                callBackAssets.Add(2, assets);
                operationHandleTexture = operationHandle;
            }
        ));

        callBack?.Invoke(callBackAssets, operationHandleGameObject, operationHandleMaterial, operationHandleTexture);
    }

    private async Task OnLoadWithLabelsOperationHandle<T>(List<string> KeysLabels, Addressables.MergeMode mergeMode, UnityAction<List<object>, AsyncOperationHandle<IList<T>>> callBack)
    {
        List<object> assetData = new List<object>();
        AsyncOperationHandle<IList<T>> operationHandle;
        operationHandle = Addressables.LoadAssetsAsync<T>(
            KeysLabels,
            addressable =>
            {
                if (addressable != null)
                {
                    assetData.Add(addressable);
                    print($"> Name : {addressable.ToString()}");
                }
            },
            mergeMode,
            false
        );

        await operationHandle.Task;
        callBack?.Invoke(assetData, operationHandle);
    }
    
    private void OnLoadWithLabelsCompleted<T>(AsyncOperationHandle<IList<T>> OperationHandle, List<object> Assets)
    {
        /*
        if (OperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.LogWarning("Some assets did not load.");
            
            print($"G = {testtt[0].Count} || M = {testtt[1].Count} || T {testtt[2].Count}");
            
            foreach (object aseet in testtt[0])
            {
                print($"Name : {((GameObject)aseet).name}");
            }
            
            foreach (object aseet in testtt[1])
            {
                print($"Name : {((Material)aseet).name}");
            }
            
            foreach (object aseet in testtt[2])
            {
                print($"Name : {((Texture)aseet).name}");
            }
        }*/
    }

    private void OnDestroy()
    {
        
    }
}
