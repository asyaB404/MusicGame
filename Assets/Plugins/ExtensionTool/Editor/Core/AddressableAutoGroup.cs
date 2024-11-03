using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.Util;

public static class AddressableAutoGroup
{
    /// <summary>
    /// 清理所有分组内的资源
    /// </summary>
    [MenuItem("AddressableExtension/移除分组")]
    public static void RemoveGroups()
    {
        var startTime = EditorApplication.timeSinceStartup;
        try
        {
            AssetDatabase.StartAssetEditing();
            var setting = AddressableAssetSettingsDefaultObject.Settings;
            if (setting == null)
            {
                Debug.LogError("AddressableAssetSettingsDefaultObject.Settings is null");
                return;
            }

            var listGroups = setting.groups;
            for (var i = listGroups.Count - 1; i >= 0; i--)
            {
                setting.RemoveGroup(listGroups[i]);
            }

            Debug.Log("Addressable Group移除完毕");
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            throw;
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
        }

        var endTime = EditorApplication.timeSinceStartup;
        Debug.Log($"Addressable Group移除完毕 time:{endTime - startTime}");
    }

    /// <summary>
    /// 自动设置分组
    /// </summary>
    [MenuItem("AddressableExtension/自动分组")]
    public static void AutoGroup()
    {
        RemoveGroups();
        var assetPathList = AddressableDef.AddressableAssetsRootFolder.GetAllFilePaths();
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            Debug.LogError("找不到addressable设置文件");
            return;
        }

        var startTime = EditorApplication.timeSinceStartup;
        try
        {
            AssetDatabase.StartAssetEditing();
            Debug.Log($"资源数量:{assetPathList.Count}");
            foreach (var assetPath in assetPathList)
            {
                var assetLocalPath = assetPath.GetUnityLocalAssetFolder();
                //当前资源应该被分配的组名
                var groupName = AddressableUtil.GetGroupName(assetLocalPath);
                var group = settings.FindGroup(groupName);
                //创建不存在的分组
                if (@group == null)
                {
                    @group = CreateGroup(groupName);
                }

                var guid = AssetDatabase.AssetPathToGUID(assetLocalPath);
                var entry = @group.GetAssetEntry(guid) ?? settings.CreateOrMoveEntry(guid, @group, false, false);
                entry.address = assetLocalPath;
                entry.SetLabel(@group.Name, true, false, false);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            throw;
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
        }

        var endTime = EditorApplication.timeSinceStartup;
        Debug.Log($"自动分组完毕 time:{endTime - startTime}");
    }

    /// <summary>
    /// 创建分组
    /// </summary>
    /// <param name="groupName"></param>
    private static AddressableAssetGroup CreateGroup(string groupName)
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        Assert.IsNotNull(settings);
        var group = settings.CreateGroup(groupName, false, false, false, settings.DefaultGroup.Schemas);
        settings.AddLabel(groupName, false);
        // schema
        var schemaLoading = (BundledAssetGroupSchema)group.Schemas[1];
        schemaLoading.BuildPath.SetVariableByName(group.Settings, AddressableAssetSettings.kLocalBuildPath);
        schemaLoading.LoadPath.SetVariableByName(group.Settings, AddressableAssetSettings.kLocalLoadPath);
        schemaLoading.Compression = BundledAssetGroupSchema.BundleCompressionMode.LZ4;
        schemaLoading.ForceUniqueProvider = false;
        schemaLoading.UseAssetBundleCache = false;
        schemaLoading.UseAssetBundleCrc = false;
        schemaLoading.UseAssetBundleCrcForCachedBundles = false;
        schemaLoading.Timeout = 0;
        schemaLoading.ChunkedTransfer = false;
        schemaLoading.RedirectLimit = -1;
        schemaLoading.RetryCount = 0;
        schemaLoading.BundleMode = BundledAssetGroupSchema.BundlePackingMode.PackTogether;
        schemaLoading.BundleNaming = BundledAssetGroupSchema.BundleNamingStyle.NoHash;
        var type = schemaLoading.GetType();

        //加密包体的逻辑 待对接成2021版本
        //var fieldInfo = type.GetField("m_DataStreamProcessorType",
        //    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
        //fieldInfo?.SetValue(schemaLoading, new SerializedType { Value = typeof(RabiAesStreamProcessor) });
        return group;
    }
}