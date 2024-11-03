// ******************************************************************
//       /\ /|       @file       AddressableUtil.cs
//       \ V/        @brief      可寻址工具
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-03-14 10:19:19
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using System.Text;
using UnityEngine;

public static class AddressableUtil
{
    /// <summary>
    /// 获取某个路径下资源所属分组名
    /// </summary>
    /// <param name="assetLocalPath">Asset开头的unity本地资源路径</param>
    /// <returns></returns>
    public static string GetGroupName(string assetLocalPath)
    {
        var folderNameArray = assetLocalPath.Split('/');
        if (folderNameArray.Length < 2)
        {
            Debug.LogError($"资源所在路径长度小于2 assetLocalPath：{assetLocalPath}");
            return string.Empty;
        }

        var stringBuilder = new StringBuilder();
        for (var i = 0; i < folderNameArray.Length - 1; i++)
        {
            stringBuilder.Append(folderNameArray[i]);
            //当前不是尾项
            if (i + 1 < folderNameArray.Length - 1)
            {
                stringBuilder.Append("-");
            }
        }

        return stringBuilder.ToString();
    }
}