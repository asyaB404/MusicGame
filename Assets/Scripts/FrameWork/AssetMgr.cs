using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
/// <summary>
/// 资源加载模式，临时加载(跨场景就卸载)，永久加载
/// </summary>
public enum AssetLoadType
{
    Permanent, //永久加载，不会在切换场景时释放
    Temp //临时加载，会在切换场景时释放
}
/// <summary>
/// 资源管理器
/// </summary>
public static class AssetMgr
{
    /// <summary>
    /// 键是资源加载类型，值的键是资源在表格里的名字，值的值是加载的句柄
    /// </summary>
    private static readonly Dictionary<AssetLoadType, Dictionary<string, AsyncOperationHandle>> m_LoadTypeToHandleDict =
       new Dictionary<AssetLoadType, Dictionary<string, AsyncOperationHandle>>();

    static AssetMgr()
    {
        //创建这个类的时候 调用一下Addressables的初始化函数
        Addressables.InitializeAsync();
    }

    #region 加载资源
    /// <summary>
    /// 异步加载资源，通过等待的方式
    /// </summary>
    /// <param name="path">资源路径</param>
    /// <param name="assetLoadType">资源加载类型，临时资源和永久资源</param>
    /// <typeparam name="T">资源类型</typeparam>
    /// <returns></returns>
    public static async UniTask<T> LoadAssetAsync<T>(string path, AssetLoadType assetLoadType = AssetLoadType.Permanent) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError($"加载资源路径为空 path:{path}");
            return null;
        }

        // 如果之前字典里还不存在这种加载类型 就初始化一下键值对 
        if (!m_LoadTypeToHandleDict.ContainsKey(assetLoadType))
        {
            m_LoadTypeToHandleDict.Add(assetLoadType, new Dictionary<string, AsyncOperationHandle>());
        }
    
        // 获取到目标类型的字典
        var dict = m_LoadTypeToHandleDict[assetLoadType];

        // 如果字典里已经包含 那么就不重复加载，直接获取之前加载的
        var isRepeatLoad = dict.ContainsKey(path);
        AsyncOperationHandle<T> handle = isRepeatLoad ? dict[path].Convert<T>() : Addressables.LoadAssetAsync<T>(path);
    
        if (!isRepeatLoad) // 如果还未加载过，就将其加入字典里
        {
            dict.Add(path, handle);
        }

        // 如果之前已经加载过此资源，那么直接将结果返回
        if (handle.IsDone) return handle.Result;

        // 等待加载完成，如果加载成功就将其返回
        await handle.Task; 

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }

        // 如果加载失败，就清理一下脏数据，避免对后续加载造成影响
        Debug.LogError($"加载失败 path:{path}");
        if (dict.ContainsKey(path))
        {
            dict.Remove(path);
        }
    
        Addressables.Release(handle);
        return null;
    }


    /// <summary>
    /// 异步加载资源，通过回调函数的方式
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">资源路径，如果为空则不会加载</param>
    /// <param name="callBack">回调函数</param>
    /// <param name="assetLoadType">资源加载类型，临时资源和永久资源</param>
    public static void LoadAssetAsync<T>(string path, Action<T> callBack, AssetLoadType assetLoadType = AssetLoadType.Permanent) where T : Object
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError($"加载资源路径为空 path:{path}");
            return;
        }

        //如果之前字典里还不存在这种加载类型 就初始化一下键值对 
        if (!m_LoadTypeToHandleDict.ContainsKey(assetLoadType))
        {
            m_LoadTypeToHandleDict.Add(assetLoadType, new Dictionary<string, AsyncOperationHandle>());
        }
        //获取到目标类型的字典
        var dict = m_LoadTypeToHandleDict[assetLoadType];

        //如果字典里已经包含 那么就不重复加载，直接获取之前加载的
        var isRepeatLoad = dict.ContainsKey(path);
        var handle = isRepeatLoad ? dict[path].Convert<T>() : Addressables.LoadAssetAsync<T>(path);
        if (!isRepeatLoad)//如果还未加载过，就将其加入字典里
        {
            dict.Add(path, handle);
        }

        //如果之前已经加载完成了 就直接使用资源
        if (handle.IsDone)
        {
            callBack?.Invoke(handle.Result);
            return;
        }

        var handle1 = handle;
        handle.Completed += obj =>
        {
            //如果加载成功 就使用资源
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                callBack?.Invoke(obj.Result);
                return;
            }
            //如果加载失败 就清理脏数据 
            Debug.LogError($"加载失败 path:{path}");
            if (!dict.ContainsKey(path)) return;
            dict.Remove(path);
            Addressables.Release(handle1);
        };
    }

    /// <summary>
    /// 同步加载
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">资源路径，如果为空则不会加载</param>
    /// <param name="assetLoadType">资源加载类型，临时资源和永久资源</param>
    public static T LoadAssetSync<T>(string path, AssetLoadType assetLoadType = AssetLoadType.Permanent) where T : Object
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError($"加载资源路径为空 path:{path}");
            return null;
        }
        //如果之前字典里还不存在这种加载类型 就初始化一下键值对 
        if (!m_LoadTypeToHandleDict.ContainsKey(assetLoadType))
        {
            m_LoadTypeToHandleDict.Add(assetLoadType, new Dictionary<string, AsyncOperationHandle>());
        }
        //获取到目标类型的字典
        var dict = m_LoadTypeToHandleDict[assetLoadType];

        //如果字典里已经包含 那么就不重复加载，直接获取之前加载的
        var isRepeatLoad = dict.ContainsKey(path);
        var handle = isRepeatLoad ? dict[path].Convert<T>() : Addressables.LoadAssetAsync<T>(path);
        if (!isRepeatLoad)
        {
            dict.Add(path, handle);
        }
        var asset = handle.WaitForCompletion();
        return asset;
    }
    #endregion

    #region 加载卸载场景
    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="path">场景路径</param>
    /// <param name="loadMode"></param>
    public static async Task<SceneInstance> LoadSceneAsync(string path, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError($"加载资源路径为空 path:{path}");
            return default;
        }
        var handle = Addressables.LoadSceneAsync(path, loadMode);
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }

        Debug.LogError($"场景加载失败 path:{path}");
        //释放句柄 以及再次试图加载场景
        Addressables.Release(handle);
        return await LoadSceneAsync(path, loadMode);
    }

    /// <summary>
    /// 异步卸载场景
    /// </summary>
    /// <param name="scene"> 场景Instance引用 </param>
    public static async Task UnloadSceneAsync(SceneInstance scene)
    {
        var handle = Addressables.UnloadSceneAsync(scene);
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Addressables.Release(handle);
            return;
        }
        Debug.LogError($"场景卸载失败 scene:{scene.Scene.name}");
    }
    #endregion

    #region 释放资源
    /// <summary>
    /// 释放未在使用的资源(切换场景等环节可以调用，释放掉临时加载的资源)
    /// </summary>
    public static void ClearUnused()
    {
        ReleaseByLoadType(AssetLoadType.Temp);
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    /// <summary>
    /// 释放全部资源(感觉不可能用得上)
    /// </summary>
    public static void OnClear()
    {
        foreach (var loadType in m_LoadTypeToHandleDict.Keys)
        {
            ReleaseByLoadType(loadType);
        }
        m_LoadTypeToHandleDict.Clear();
    }

    /// <summary>
    /// 释放某种加载类型的资源
    /// </summary>
    /// <param name="assetLoadType">资源加载类型</param>
    private static void ReleaseByLoadType(AssetLoadType assetLoadType = AssetLoadType.Temp)
    {
        //如果之前字典里还不存在这种加载类型 就初始化一下键值对
        if (!m_LoadTypeToHandleDict.ContainsKey(assetLoadType))
        {
            m_LoadTypeToHandleDict.Add(assetLoadType, new Dictionary<string, AsyncOperationHandle>());
        }
        //获取到目标类型的字典
        var dict = m_LoadTypeToHandleDict[assetLoadType];
        //释放字典里的资源 以及清空字典
        foreach (var handle in dict.Values)
        {
            Addressables.Release(handle);
        }
        dict.Clear();
    }

    /// <summary>
    /// 释放单个资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">资源路径</param>
    /// <param name="assetLoadType">资源加载类型，临时资源和永久资源</param>
    public static void Release<T>(string path, AssetLoadType assetLoadType = AssetLoadType.Permanent)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogWarning($"要卸载的资源的路径为空 path:{path}");
            return;
        }

        //如果之前字典里还不存在这种加载类型 就初始化一下键值对
        if (!m_LoadTypeToHandleDict.ContainsKey(assetLoadType))
        {
            m_LoadTypeToHandleDict.Add(assetLoadType, new Dictionary<string, AsyncOperationHandle>());
        }

        //获取到目标类型的字典
        var dict = m_LoadTypeToHandleDict[assetLoadType];
        //释放句柄，并将这个键名从字典离移除
        if (!dict.ContainsKey(path)) return;
        var handle = dict[path].Convert<T>();
        Addressables.Release(handle);
        dict.Remove(path);
    }
    #endregion

   
}