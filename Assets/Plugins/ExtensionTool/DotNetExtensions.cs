using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class DotNetExtensions
{
    /// <summary>
    /// 转换成时间字符串 时:分:秒 不足10补0
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static string TimeToStringHMS(this float time)
    {
        int second = (int)time;
        string h = (second / 3600) > 9 ? (second / 3600).ToString() : "0" + (second / 3600);
        string m = (second % 3600) / 60 > 9 ? ((second % 3600) / 60).ToString() : "0" + ((second % 3600) / 60);
        string s = second % 60 > 9 ? (second % 60).ToString() : "0" + (second % 60);
        return h + ":" + m + ":" + s;
    }

    /// <summary>
    /// 转换成时间字符串 分:秒 不足10补0
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static string TimeToStringMS(this float time)
    {
        int second = (int)time;
        string m = (second / 60) > 9 ? (second / 60).ToString() : "0" + (second / 60);
        string s = second % 60 > 9 ? (second % 60).ToString() : "0" + (second % 60);
        return m + ":" + s;
    }

    /// <summary>
    /// 转换成时间字符串 分:秒:100制 05:30:88  不足10补0
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static string TimeToStringMSMS(this float time)
    {
        int second = (int)time;
        string m = (second / 60) > 9 ? (second / 60).ToString() : "0" + (second / 60);
        string s = second % 60 > 9 ? (second % 60).ToString() : "0" + (second % 60);
        int ms = (int)((time - Mathf.FloorToInt(time)) * 100);
        return m + ":" + s + ":" + (ms > 9 ? ms.ToString() : "0" + ms);
    }

    /// <summary>
    /// 向量旋转
    /// </summary>
    /// <param name="v"></param>
    /// <param name="rotateAngle">旋转角度，单位度</param>
    /// <returns></returns>
    public static Vector2 Vector2Rotate(this Vector2 v, float rotateAngle)
    {
        //先将角度换算成弧度
        rotateAngle = rotateAngle * Mathf.PI / 180;
        float cosTheta = Mathf.Cos(rotateAngle);
        float sinTheta = Mathf.Sin(rotateAngle);
        return new Vector2((v.x * cosTheta) - (v.y * sinTheta), (v.x * sinTheta) + (v.y * cosTheta));
    }

    /// <summary>
    /// 根据角度获取一个单位向量
    /// </summary>
    /// <param name="angle">向量与X轴正方向的夹角</param>
    /// <returns></returns>
    public static Vector2 GetUnitVector2ByAngle(this float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.PI / 180), Mathf.Sin(angle * Mathf.PI / 180));
    }

    /// <summary>
    /// 根据角度获取一个单位向量
    /// </summary>
    /// <param name="angle">向量与X轴正方向的夹角</param>
    /// <returns></returns>
    public static Vector2 GetUnitVector2ByAngle(this int angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.PI / 180), Mathf.Sin(angle * Mathf.PI / 180));
    }

    /// <summary>
    /// 从上下左右四维向量里面随机得到一个二维向量(在某个范围内随机得到一点)
    /// </summary>
    /// <param name="v4">四个分量为 上下左右</param>
    /// <returns></returns>
    public static Vector2 GetRandomV2FromV4(this Vector4 v4)
    {
        //左     右                            //下    上
        return new Vector2(UnityEngine.Random.Range(v4.z, v4.w), UnityEngine.Random.Range(v4.y, v4.x));
    }

    /// <summary>
    /// 将角度映射到0-360
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static float RemapAngle360(this float angle)
    {
        return ((angle % 360) + 360) % 360;
    }

    /// <summary>
    /// 将角度映射到0-360
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static float RemapAngle360(this int angle)
    {
        return ((angle % 360) + 360) % 360;
    }

    /// <summary>
    /// 将当前数字映射到范围内
    /// </summary>
    /// <param name="index">当前索引</param>
    /// <param name="count">数组长度</param>
    /// <returns></returns>
    public static int MapIndex(this int index, int count)
    {
        return ((index % count) + count) % count;
    }

    /// <summary>
    /// 在数组的索引范围内随机获取一个索引
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static int GetRandomIndex<T>(this List<T> list)
    {
        return UnityEngine.Random.Range(0, list.Count);
    }

    /// <summary>
    /// 随机获取一个位于X,Y之间的值
    /// </summary>
    /// <param name="range">随机范围</param>
    /// <returns></returns>
    public static float GetRandomFormV2(this Vector2 range)
    {
        if (range.y > range.x)
        {
            return UnityEngine.Random.Range(range.x, range.y);
        }
        else
        {
            return UnityEngine.Random.Range(range.y, range.x);
        }
    }

    /// <summary>
    /// 随机获取一个位于X,Y之间的值
    /// </summary>
    /// <param name="range">随机范围</param>
    /// <returns></returns>
    public static int GetRandomIntFormV2(this Vector2 range)
    {
        if (range.y > range.x)
        {
            return UnityEngine.Random.Range((int)range.x, (int)range.y);
        }
        else
        {
            return UnityEngine.Random.Range((int)range.y, (int)range.x);
        }
    }

    #region 数组 集合 字典

    /// <summary>
    /// 从数组里随机获取一个跟给定的元素不同的元素(如果没有给定元素则无要求)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="remove">是否将随机到的元素从原数组里移除</param>
    /// <param name="conflict">希望屏蔽掉的元素</param>
    /// <returns></returns>
    public static T RandomFromList<T>(this List<T> list, bool remove = true, T conflict = null)
        where T : UnityEngine.Object
    {
        //随机获取一个元素
        int index = UnityEngine.Random.Range(0, list.Count);
        T temp = list[index];
        //如果元素与想要屏蔽掉的元素相同 就重新随机一个
        if (temp == conflict)
        {
            return RandomFromList(list, conflict);
        }

        //如果需要将元素从原数组里移除 就移除一下
        if (remove)
        {
            list.RemoveAt(index);
        }

        return temp;
    }

    /// <summary>
    /// 从数组里随机获取一组元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="count">获取元素的数量</param>
    /// <param name="remove">是否将随机到的元素从数组里移除</param>
    /// <returns></returns>
    public static List<T> RandomFromList<T>(this List<T> list, int count, bool remove = false)
    {
        //如果数组里的元素比需要的元素数量少 或者需要的元素数量为0的话 就返回个空数组
        if (list.Count < count || count <= 0)
        {
            return new List<T>();
        }

        //创建一个存放结果的数组
        List<T> result = new List<T>(count);
        //如果不移除 那么就拷贝一份数组，否则就用原数组 
        List<T> temp = !remove ? new List<T>(list) : list;
        for (int i = 0; i < count; i++)
        {
            T t = temp[UnityEngine.Random.Range(0, temp.Count)];
            temp.Remove(t);
            result.Add(t);
        }

        return result;
    }

    /// <summary>
    /// 从数组里随机获取一个元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values"></param>
    /// <returns></returns>
    public static T RandomFromArray<T>(this T[] values)
    {
        return values[UnityEngine.Random.Range(0, values.Length)];
    }

    /// <summary>
    /// 返回一个打乱后的数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<T> Shuffle<T>(this List<T> list)
    {
        List<T> temp = new List<T>();
        List<int> indexList = list.Count.GetRandomIndexList();
        for (int i = 0; i < indexList.Count; i++)
        {
            temp.Add(list[indexList[i]]);
        }

        return temp;
    }

    /// <summary>
    /// 返回一组打乱的(0至数组count-1)的索引
    /// </summary>
    /// <returns></returns>
    public static List<int> GetRandomIndexList<T>(this List<T> selfList)
    {
        return selfList.Count.GetRandomIndexList();
    }

    /// <summary>
    /// 返回一组打乱的(0至count-1)的索引
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public static List<int> GetRandomIndexList(this int count)
    {
        //构建一个存放索引的数组 和一个存放结果的数组
        List<int> list = new List<int>(count);
        List<int> resule = new List<int>();
        for (int i = 0; i < count; i++)
        {
            list.Add(i);
        }

        for (int i = 0; i < count; i++)
        {
            int index = UnityEngine.Random.Range(0, list.Count);
            resule.Add(list[index]);
            list.RemoveAt(index);
        }

        return resule;
    }

    /// <summary>
    /// 如果数组里不包含此元素就将此元素加入数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="t">要添加的新元素</param>
    /// <returns></returns>
    public static List<T> AddWithoutRepetition<T>(this List<T> list, T t)
    {
        if (!list.Contains(t))
        {
            list.Add(t);
        }

        return list;
    }

    /// <summary>
    /// 判断两个集合是否相同
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list1"></param>
    /// <param name="list2"></param>
    /// <returns></returns>
    public static bool IsEqual<T>(this List<T> list1, List<T> list2)
    {
        bool equal = true;
        //数组1里有数组2里没有的元素 或者数组2里有数组1里没有的元素 就说明两个数组不相同
        list1.ForEach(a =>
        {
            if (!list2.Contains(a))
            {
                equal = false;
            }
        });
        list2.ForEach(a =>
        {
            if (!list1.Contains(a))
            {
                equal = false;
            }
        });
        return equal;
    }

    /// <summary>
    /// 对数组元素求和
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static int Sum(this List<int> list)
    {
        int total = 0;
        list.ForEach(a => total += a);
        return total;
    }

    /// <summary>
    /// 对数组元素求和
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static float Sum(this List<float> list)
    {
        float total = 0;
        list.ForEach(a => total += a);
        return total;
    }

    /// <summary>
    /// 对数组元素求和
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static double Sum(this List<double> list)
    {
        double total = 0;
        list.ForEach(a => total += a);
        return total;
    }

    /// <summary>
    /// 获取数组里某个索引的值，如果索引超出上限就返回默认值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selfList"></param>
    /// <param name="index">获取哪个索引的元素</param>
    /// <returns></returns>
    public static T TryGetValue<T>(this List<T> selfList, int index)
    {
        return selfList.Count > index ? selfList[index] : default;
    }

    /// <summary>
    /// 获取字典里某个键的值，如果不存在这个键就返回默认值
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="dic"></param>
    /// <param name="key">获取哪个键的元素</param>
    /// <returns></returns>
    public static T TryGetValue<K, T>(this Dictionary<K, T> dic, K key)
    {
        if (dic.ContainsKey(key))
        {
            return dic[key];
        }

        return default;
    }

    /// <summary>
    /// 移除数组里的某个元素，如果他存在
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="target">要移除的元素</param>
    public static void RemoveSafe<T>(this List<T> list, T target)
    {
        if (list.Contains(target))
        {
            list.Remove(target);
        }
    }

    /// <summary>
    /// 移除字典里的某个键，如果他存在
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="dic"></param>
    /// <param name="targetKey">要移除的键</param>
    public static void RemoveSafe<K, V>(this Dictionary<K, V> dic, K targetKey)
    {
        if (dic.ContainsKey(targetKey))
        {
            dic.Remove(targetKey);
        }
    }

    /// <summary>
    /// 写法更加便捷的Foreach
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selfArray"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> selfArray, Action<T> action)
    {
        if (action == null) return selfArray;
        foreach (var item in selfArray)
        {
            action(item);
        }

        return selfArray;
    }

    /// <summary>
    /// 写法更加便捷的倒序Foreach
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selfList"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static List<T> ForEachReverse<T>(this List<T> selfList, Action<T> action)
    {
        if (action == null) return selfList;

        for (var i = selfList.Count - 1; i >= 0; i--)
        {
            action(selfList[i]);
        }

        return selfList;
    }

    /// <summary>
    /// 写法更加便捷的倒序Foreach
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selfList"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static List<T> ForEachReverse<T>(this List<T> selfList, Action<T, int> action)
    {
        if (action == null) return selfList;

        for (var i = selfList.Count - 1; i >= 0; i--)
        {
            action(selfList[i], i);
        }

        return selfList;
    }

    /// <summary>
    /// 如果字典里取不到值，就返回我们给定的默认值
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key">键</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns></returns>
    public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
        TValue defaultValue = default)
    {
        TValue value;
        return dictionary.TryGetValue(key, out value) ? value : defaultValue;
    }

    /// <summary>
    /// 如果字典里取不到值，就通过我们给的方法创建一个值返回
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key">键</param>
    /// <param name="createFun">创建默认值的方法</param>
    /// <returns></returns>
    public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
        Func<TValue> createFun)
    {
        TValue value;
        return dictionary.TryGetValue(key, out value) ? value : createFun();
    }

    /// <summary>
    /// 如果字典里不包含这个键，就将这对键值添加进去
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key">键名</param>
    /// <param name="value">值</param>
    public static void AddSafe<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
        TValue value = default)
    {
        if (!dictionary.ContainsKey(key))
        {
            dictionary.Add(key, value);
        }
    }

    /// <summary>
    /// 合并两个字典
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="dic1"></param>
    /// <param name="dic2"></param>
    /// <returns></returns>
    public static Dictionary<K, V> AddRange<K, V>(this Dictionary<K, V> dic1, Dictionary<K, V> dic2)
    {
        foreach (var item in dic2)
        {
            if (!dic1.ContainsKey(item.Key))
            {
                dic1.Add(item.Key, item.Value);
            }
        }

        return dic1;
    }

    /// <summary>
    /// 合并两个数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array1"></param>
    /// <param name="array2"></param>
    /// <returns></returns>
    public static T[] AddRange<T>(this T[] array1, T[] array2)
    {
        T[] temp = new T[array1.Length + array2.Length];
        int index = 0;
        foreach (var item in array1)
        {
            temp[index] = item;
            index++;
        }

        foreach (var item in array2)
        {
            temp[index] = item;
            index++;
        }

        return temp;
    }

    /// <summary>
    /// 根据给定键名从一组参数里面找到想要的参数  键名:值 
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="key">键名</param>
    /// <returns></returns>
    public static string GetValueByKey(this object[] objs, string key)
    {
        string value = "";
        if (objs == null || objs.Length == 0) return value;

        foreach (var item in objs)
        {
            string str = item.ToString();
            if (!str.Contains(key)) continue;
            value = str.Split(str.Contains("：") ? '：' : ':')[1];
            break;
        }

        return value;
    }

    /// <summary>
    /// 根据给定键名从一组参数里面找到想要的参数  键名:值 
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="key">键名</param>
    /// <returns></returns>
    public static string GetValueByKey(this List<object> objs, string key)
    {
        string value = "";
        if (objs == null || objs.Count == 0) return value;

        foreach (var item in objs)
        {
            string str = item.ToString();
            if (!str.Contains(key)) continue;
            value = str.Split(str.Contains("：") ? '：' : ':')[1];
            break;
        }

        return value;
    }

    /// <summary>
    /// 将数组里找到的第一个类型匹配上的元素返回
    /// </summary>
    /// <param name="arg"></param>
    /// <returns></returns>
    public static T GetTargetTypeValue<T>(this object[] arg)
    {
        T t = default;
        if (arg == null || arg.Length == 0) return t;

        //遍历数组 找到目标类型的数据返回        
        foreach (var item in arg)
        {
            if (item is T tt)
            {
                t = tt;
                break;
            }
        }

        return t;
    }

    /// <summary>
    /// 将数组里找到的第一个类型匹配上的元素返回
    /// </summary>
    /// <param name="arg"></param>
    /// <returns></returns>
    public static T GetTargetTypeValue<T>(this List<object> arg)
    {
        T t = default;
        if (arg == null || arg.Count == 0) return t;

        //遍历数组 找到目标类型的数据返回        
        foreach (var item in arg)
        {
            if (item is T tt)
            {
                t = tt;
                break;
            }
        }

        return t;
    }

    #endregion
}

public static class IOExtension
{
    /// <summary>
    /// 如果某个文件的文件夹还不存在 就将其创建出来
    /// </summary>
    /// <param name="filepath"></param>
    public static void CreatFileDirectory(this string filepath)
    {
        //获取路径里的文件夹名字
        string newPathDir = Path.GetDirectoryName(filepath);
        //将文件夹创建出来
        CreateDirIfNotExists(newPathDir);
    }

    /// <summary>
    /// 如果目标文件夹不存在就创建出来
    /// </summary>
    public static string CreateDirIfNotExists(this string dirFullPath)
    {
        if (!Directory.Exists(dirFullPath))
        {
            Directory.CreateDirectory(dirFullPath);
        }

        return dirFullPath;
    }

    /// <summary>
    /// 删除文件 如果存在
    /// </summary>
    /// <param name="fileFullPath"></param>
    /// <returns> True if exists</returns>
    public static bool DeleteFileIfExists(this string fileFullPath)
    {
        if (File.Exists(fileFullPath))
        {
            File.Delete(fileFullPath);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 删除文件夹，如果存在
    /// </summary>
    public static bool DeleteDirIfExists(this string dirFullPath)
    {
        if (Directory.Exists(dirFullPath))
        {
            Directory.Delete(dirFullPath, true);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 清空文件夹，如果存在。
    /// </summary>
    public static void ClearDirIfExists(this string dirFullPath)
    {
        //删除整个文件夹 然后创建一个空的文件夹
        if (Directory.Exists(dirFullPath))
        {
            Directory.Delete(dirFullPath, true);
        }

        Directory.CreateDirectory(dirFullPath);
    }

    /// <summary>
    /// 保存文本
    /// </summary>
    /// <param name="text"></param>
    /// <param name="path">文件路径</param>
    public static void SaveText(this string text, string path)
    {
        //先删除可能存在的文件
        path.DeleteFileIfExists();
        using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            using (var sr = new StreamWriter(fs))
            {
                sr.Write(text); //开始写入值
            }
        }
    }

    /// <summary>
    /// 保存文件数据(二进制形式)
    /// </summary>
    /// <param name="path">全路径</param>
    /// <param name="_data">数据</param>
    /// <returns></returns>
    public static bool SaveText2Binary(this string _data, string path)
    {
        //将字符串转成UTF-8格式的byte数组
        byte[] dataByte = Encoding.GetEncoding("UTF-8").GetBytes(_data);
        //用byte数组创建文件
        return CreateFile(path, dataByte);
    }

    /// <summary>
    /// 读取文本
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static string ReadText(this FileInfo file)
    {
        return ReadText(file.FullName);
    }

    /// <summary>
    /// 读取文本
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static string ReadText(this string fileFullPath)
    {
        if (!File.Exists(fileFullPath)) return "";

        var result = string.Empty;
        using (var fs = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read))
        {
            using (var sr = new StreamReader(fs))
            {
                result = sr.ReadToEnd();
            }
        }

        return result;
    }

    /// <summary>
    /// 使路径标准化，去除空格并将所有'\'转换为'/'
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string MakePathStandard(this string path)
    {
        return path.Trim().Replace("\\", "/");
    }

    /// <summary>
    /// 获取文件夹名
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetDirectoryName(this string filePath)
    {
        filePath = MakePathStandard(filePath);
        return filePath.Substring(0, filePath.LastIndexOf('/'));
    }

    /// <summary>
    /// 获取文件名
    /// </summary>
    /// <param name="path"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string GetFileName(this string path, char separator = '/')
    {
        path = MakePathStandard(path);
        return path.Substring(path.LastIndexOf(separator) + 1);
    }

    /// <summary>
    /// 获取文件扩展名 包括.
    /// </summary>
    /// <param name="absOrAssetsPath"></param>
    /// <returns></returns>
    public static string GetFileExtendName(this string absOrAssetsPath)
    {
        var lastIndex = absOrAssetsPath.LastIndexOf(".");

        if (lastIndex >= 0)
        {
            return absOrAssetsPath.Substring(lastIndex);
        }

        return string.Empty;
    }

    /// <summary>
    /// 修改 文件名
    /// </summary>
    /// <param name="path"></param>
    /// <param name="newName"></param>
    public static void ChangeFileName(this string path, string newName)
    {
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Move(path, newName);
        }
    }

    /// <summary>
    /// 获取不带后缀的文件名
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string GetFileNameWithoutExtention(this string filePath, char separator = '/')
    {
        //先获取到文件名 然后再将文件后缀去掉
        return GetFilePathWithoutExtention(GetFileName(filePath, separator));
    }

    /// <summary>
    /// 获取不带后缀的文件路径
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetFilePathWithoutExtention(this string filePath)
    {
        //截取路径到 . 为止
        if (filePath.Contains("."))
        {
            return filePath.Substring(0, filePath.LastIndexOf('.'));
        }

        return filePath;
    }

    /// <summary>
    /// 让给定路径的文件夹或者文件的目录存在
    /// </summary>
    /// <param name="path"></param>
    public static void MakeFileDirectoryExist(this string directoryName)
    {
        string root = Path.GetDirectoryName(directoryName);
        if (!Directory.Exists(root))
        {
            Debug.Log(1);
            Directory.CreateDirectory(root);
        }

        Debug.Log(root);
    }

    /// <summary>
    /// 结合目录
    /// </summary>
    /// <param name="paths"></param>
    /// <returns></returns>
    public static string Combine(this string[] paths)
    {
        string result = "";
        foreach (string path in paths)
        {
            result = Path.Combine(result, path);
        }

        result = MakePathStandard(result);
        return result;
    }

    /// <summary>
    /// 结合目录
    /// </summary>
    /// <param name="paths"></param>
    /// <returns></returns>
    public static string Combine(this List<string> paths)
    {
        string result = "";
        foreach (string path in paths)
        {
            result = Path.Combine(result, path);
        }

        result = MakePathStandard(result);
        return result;
    }

    /// <summary>
    /// 获取某个文件夹里 全部子文件夹的名字，不递归
    /// </summary>
    /// <param name="dirABSPath"></param>
    /// <returns></returns>
    public static List<string> GetDirSubDirNameList(this string dirABSPath)
    {
        var di = new DirectoryInfo(dirABSPath);
        //获取全部子文件夹信息
        var dirs = di.GetDirectories();
        //将名字构建成数组返回出去
        return dirs.Select(d => d.Name).ToList();
    }

    /// <summary>
    /// 获取某个文件夹里 全部子文件夹的全路径名字，不递归
    /// </summary>
    /// <param name="dirABSPath"></param>
    /// <returns></returns>
    public static List<string> GetDirSubDirFullNameList(this string dirABSPath)
    {
        var di = new DirectoryInfo(dirABSPath);
        //获取全部子文件夹信息
        var dirs = di.GetDirectories();
        //将名字构建成数组返回出去
        return dirs.Select(d => d.FullName).ToList();
    }

    /// <summary>
    /// 获取文件夹里面全部文件的路径
    /// </summary>
    /// <param name="dirABSPath"></param>
    /// <param name="isRecursive">递归获取</param>
    /// <param name="suffix">后缀要求，不填则代表任意类型资源</param>
    /// <returns></returns>
    public static List<string> GetDirSubFilePathList(this string dirABSPath, bool isRecursive = true,
        string suffix = "")
    {
        //获取目标文件夹的信息 如果文件夹不存在则直接返回new的新数组
        var pathList = new List<string>();
        var di = new DirectoryInfo(dirABSPath);
        if (!di.Exists)
        {
            return pathList;
        }

        var files = di.GetFiles();
        foreach (var fi in files)
        {
            //如果后缀要求不为空 且文件后缀不相匹配 就跳过
            if (!string.IsNullOrEmpty(suffix) &&
                !fi.FullName.EndsWith(suffix, System.StringComparison.CurrentCultureIgnoreCase))
            {
                continue;
            }

            pathList.Add(fi.FullName);
        }

        if (isRecursive)
        {
            //如果递归 就获取全部的文件夹 然后对文件夹进行同样的操作 且将返回的文件夹数组加到数组里
            var dirs = di.GetDirectories();
            foreach (var d in dirs)
            {
                pathList.AddRange(GetDirSubFilePathList(d.FullName, isRecursive, suffix));
            }
        }

        return pathList;
    }

    /// <summary> 
    /// 判断是否是不带 BOM 的 UTF8 格式(看不懂~) 
    /// </summary> 
    /// <param name="data"></param> 
    /// <returns></returns> 
    private static bool IsUTF8Bytes(this byte[] data)
    {
        int charByteCounter = 1;
        //计算当前正分析的字符应还有的字节数 
        byte curByte; //当前分析的字节. 
        for (int i = 0; i < data.Length; i++)
        {
            curByte = data[i];
            if (charByteCounter == 1)
            {
                if (curByte >= 0x80)
                {
                    //判断当前 
                    while (((curByte <<= 1) & 0x80) != 0)
                    {
                        charByteCounter++;
                    }

                    //标记位首位若为非0 则至少以2个1开始 如:110XXXXX......1111110X　 
                    if (charByteCounter == 1 || charByteCounter > 6)
                    {
                        return false;
                    }
                }
            }
            else
            {
                //若是UTF-8 此时第一位必须为1 
                if ((curByte & 0xC0) != 0x80)
                {
                    return false;
                }

                charByteCounter--;
            }
        }

        if (charByteCounter > 1)
        {
            throw new Exception("非预期的byte格式");
        }

        return true;
    }

    /// <summary>
    /// 递归处理某个文件夹内全部目标类型的文件
    /// </summary>
    /// <param name="path">文件夹路径</param>
    /// <param name="expandName">要处理的特定拓展名，不需要加 .</param>
    /// <param name="handle">处理函数</param>
    public static void RecursionFileHandle(this string path, string expandName, Action<string> handle)
    {
        //获取文件夹里全部的文件
        string[] files = Directory.GetFiles(path);
        foreach (var item in files)
        {
            try
            {
                //如果填写了扩展名 那么就只处理目标扩展名类型的文件
                if (!string.IsNullOrEmpty(expandName))
                {
                    if (item.EndsWith("." + expandName))
                    {
                        handle(item);
                    }
                }
                else
                {
                    handle(item);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("RecursionFileExecute Error :" + item + " Exception:" + e.ToString());
            }
        }

        //获取文件夹里全部的文件夹 然后对齐进行递归操作
        string[] dires = Directory.GetDirectories(path);
        for (int i = 0; i < dires.Length; i++)
        {
            RecursionFileHandle(dires[i], expandName, handle);
        }
    }

    /// <summary>
    /// 获取上一级目录
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetUpperPath(this string path)
    {
        int index = path.LastIndexOf('/');

        if (index != -1)
        {
            return path.Substring(0, index);
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 复制文件夹（及文件夹下所有子文件夹和文件）
    /// </summary>
    /// <param name="sourcePath">待复制的文件夹路径</param>
    /// <param name="destinationPath">目标路径</param>
    public static void CopyDirectory(this string sourcePath, string destinationPath)
    {
        //先将目标文件夹创建出来
        destinationPath.CreateDirIfNotExists();

        DirectoryInfo info = new DirectoryInfo(sourcePath);
        foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
        {
            string destName = Path.Combine(destinationPath, fsi.Name);
            //如果是文件，复制文件
            if (fsi is FileInfo)
            {
                File.Copy(fsi.FullName, destName);
            }
            //如果是文件夹，递归
            else
            {
                fsi.FullName.CopyDirectory(destName);
            }
        }
    }

    /// <summary>
    /// 用传入的byte数组创建文件
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="_data">数据</param>
    /// <returns></returns>
    public static bool CreateFile(this string path, byte[] _data)
    {
        if (string.IsNullOrEmpty(path)) return false;

        //如果文件夹不存在 先创建文件夹
        string temp = Path.GetDirectoryName(path);
        if (!Directory.Exists(temp))
        {
            Directory.CreateDirectory(temp);
        }

        try
        {
            //如果已经存在此文件 会先删除旧的文件
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            //写入数据
            using (FileStream stream = new FileStream(path.ToString(), FileMode.OpenOrCreate))
            {
                stream.Write(_data, 0, _data.Length);
                stream.Flush();
                stream.Close();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("File written fail: " + path + "  ---:" + e);
            return false;
        }

        return true;
    }

    /// <summary>
    /// 读取二进制文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static byte[] LoadByteFileByPath(this string path)
    {
        if (!File.Exists(path))
        {
            Debug.Log("path dont exists ! : " + path);
            return null;
        }

        FileStream fs = new FileStream(path, FileMode.Open);

        byte[] array = new byte[fs.Length];

        fs.Read(array, 0, array.Length);
        fs.Close();

        return array;
    }

    /// <summary>
    /// 获取unity本地资源目录(相对路径)
    /// </summary>
    /// <param name="assetPath"></param>
    /// <returns></returns>
    public static string GetUnityLocalAssetFolder(this string assetPath)
    {
        if (string.IsNullOrEmpty(assetPath))
        {
            return string.Empty;
        }

        assetPath = assetPath.MakePathStandard();
        var index = assetPath.IndexOf("Assets/", StringComparison.CurrentCultureIgnoreCase);
        return index < 0 ? string.Empty : assetPath.Substring(index);
    }

    /// <summary>
    /// 获取某个文件夹内所有非meta文件的地址
    /// </summary>
    /// <param name="modelFolder"></param>
    /// <returns></returns>
    public static List<string> GetAllFilePaths(this string modelFolder)
    {
        //全部目录
        var allFolders = GetAllFolders(modelFolder);
        var assetPathList = new List<string>();
        foreach (var filePaths in allFolders.Select(Directory.GetFiles))
        {
            assetPathList.AddRange(filePaths.Where(t => !t.EndsWith(".meta")));
        }

        return assetPathList;
    }

    /// <summary>
    /// 获取某个目录下全部子目录和文件路径
    /// </summary>
    /// <param name="modelFolder"></param>
    /// <returns></returns>
    public static List<string> GetAssetPathsAndFolders(this string modelFolder)
    {
        //全部目录
        var allFolders = GetAllFolders(modelFolder);
        var assetPathList = new List<string>();
        foreach (var filePaths in allFolders.Select(Directory.GetFiles))
        {
            assetPathList.AddRange(filePaths.Where(t => !t.EndsWith(".meta")));
        }

        assetPathList.AddRange(allFolders);
        return assetPathList;
    }

    /// <summary>
    /// 获取全部目录名称列表
    /// </summary>
    /// <param name="modelFolder"></param>
    /// <returns></returns>
    public static List<string> GetAllFolders(this string modelFolder)
    {
        var allDirs = new List<string> { GetDirectoryName(modelFolder) };
        var subDirList = GetSubFoldersRecursive(modelFolder);
        allDirs.AddRange(subDirList);
        return allDirs;
    }

    /// <summary>
    /// 递归获取所有子文件夹目录 不包含自身
    /// </summary>
    /// <param name="folder"></param>
    /// <returns></returns>
    private static IEnumerable<string> GetSubFoldersRecursive(this string folder)
    {
        var subFolders = new List<string>();
        if (string.IsNullOrEmpty(folder))
        {
            return subFolders;
        }

        //获取这个文件夹内的全部文件夹
        var folderArray = Directory.GetDirectories(folder);
        if (folderArray.Length <= 0)
        {
            return subFolders;
        }

        //将其加入数组 然后获取全部文件夹内部的全部文件夹
        subFolders.AddRange(folderArray);
        foreach (var t in folderArray)
        {
            var list = GetSubFoldersRecursive(t);
            subFolders.AddRange(list);
        }

        return subFolders;
    }
}

public static class ObjectExtention
{
    /// <summary>
    /// object转整形
    /// </summary>
    /// <param name="obj">要强转的对象</param>
    /// <param name="defaulValue">默认值</param>
    /// <returns></returns>
    public static int ToInt(this object obj, int defaulValue = 0)
    {
        return obj.ToString().ToInt(defaulValue);
    }

    /// <summary>
    /// object转小数，兼容不同区域小数表示不一样的情况
    /// </summary>
    /// <param name="obj">要强转的对象</param>
    /// <param name="defaulValue">默认值</param>
    /// <returns></returns>
    public static float ToFloat(this object obj, float defaulValue = 0)
    {
        return obj.ToString().ToFloat(defaulValue);
    }

    /// <summary>
    /// object转布尔
    /// </summary>
    /// <param name="obj">要强转的对象</param>
    /// <returns></returns>
    public static bool ToBool(this object obj, bool defaulValue = false)
    {
        return obj.ToString().ToBool(defaulValue);
    }

    /// <summary>
    /// object转二维向量
    /// </summary>
    /// <param name="obj">要强转的对象</param>
    /// <param name="defaultValue">默认值</param>
    /// <param name="safe">安全模式，安全模式会校验一下字符串，去除掉括号等用不到的符号</param>
    /// <returns></returns>
    public static Vector2 ToVector2(this object obj, Vector2 defaultValue = default, bool safe = true)
    {
        return obj.ToString().ToVector2(defaultValue, safe);
    }

    /// <summary>
    /// object转三维向量
    /// </summary>
    /// <param name="obj">要强转的对象</param>
    /// <param name="defaultValue">默认值</param>
    /// <param name="safe">安全模式，安全模式会校验一下字符串，去除掉括号等用不到的符号</param>
    /// <returns></returns>
    public static Vector3 ToVector3(this object obj, Vector3 defaultValue = default, bool safe = true)
    {
        return obj.ToString().ToVector3(defaultValue, safe);
    }

    /// <summary>
    /// object转四维向量
    /// </summary>
    /// <param name="obj">要强转的对象</param>
    /// <param name="defaultValue">默认值</param>
    /// <param name="safe">安全模式，安全模式会校验一下字符串，去除掉括号等用不到的符号</param>
    /// <returns></returns>
    public static Vector4 ToVector4(this object obj, Vector4 defaultValue = default, bool safe = true)
    {
        return obj.ToString().ToVector4(defaultValue, safe);
    }

    /// <summary>
    /// object转颜色
    /// </summary>
    /// <param name="obj">要强转的对象</param>
    /// <param name="defaultValue">默认值</param>
    /// <param name="safe">安全模式，安全模式会校验一下字符串，去除掉括号等用不到的符号</param>
    /// <returns></returns>
    public static Color ToColor(this object obj, Color defaultValue = new Color(), bool safe = true)
    {
        return obj.ToString().ToColor(defaultValue, safe);
    }

    /// <summary>
    /// 改变数据类型
    /// </summary>
    /// <typeparam name="T">目标数据类型</typeparam>
    /// <param name="value">值</param>
    /// <returns></returns>
    public static object ChangeType<T>(this object value)
    {
        if (typeof(T) == typeof(Vector2))
        {
            return value.ToVector2();
        }
        else if (typeof(T) == typeof(Vector3))
        {
            return value.ToVector3();
        }
        else if (typeof(T) == typeof(Vector4))
        {
            return value.ToVector4();
        }
        else if (typeof(T) == typeof(Color))
        {
            return value.ToColor();
        }
        else if (typeof(T) == typeof(int))
        {
            return value.ToInt();
        }
        else if (typeof(T) == typeof(float))
        {
            return value.ToFloat();
        }
        else if (typeof(T) == typeof(double))
        {
            return value.ToFloat();
        }
        else
        {
            return value.ToString();
        }
    }
}

public static class StringExtention
{
    /// <summary>
    /// 获取字符串里的数字
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static float GetNumbInString(this string str)
    {
        return System.Text.RegularExpressions.Regex.Replace(str, @"[^0-9]+", "").ToFloat();
    }

    /// <summary>
    /// 获取资源名字
    /// </summary>
    /// <param name="assetPath"></param>
    /// <returns></returns>
    public static string GetAssetName(this string assetPath)
    {
        return Path.GetFileNameWithoutExtension(assetPath);
    }

    /// <summary>
    /// 恢复换行符
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string RecoverEnter(this string str)
    {
        return str.Replace("\\n", "\n");
    }

    /// <summary> Html颜色转换为Color </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static Color GetColor(this string hex)
    {
        // 默认黑色
        if (string.IsNullOrEmpty(hex)) return Color.black;
        // 转换颜色
        hex = hex.ToLower();
        //如果不是16进制的颜色
        if (hex.IndexOf("#", StringComparison.Ordinal) != 0 || hex.Length != 7)
        {
            switch (hex)
            {
                case "red": return Color.red;
                case "green": return Color.green;
                case "blue": return Color.blue;
                case "yellow": return Color.yellow;
                case "black": return Color.black;
                case "white": return Color.white;
                case "cyan": return Color.cyan;
                case "gray": return Color.gray;
                case "grey": return Color.grey;
                case "magenta": return Color.magenta;
                default: return Color.black;
            }
        }

        //16进制颜色
        var r = Convert.ToInt32(hex.Substring(1, 2), 16);
        var g = Convert.ToInt32(hex.Substring(3, 2), 16);
        var b = Convert.ToInt32(hex.Substring(5, 2), 16);
        return new Color(r / 255f, g / 255f, b / 255f);
    }

    /// <summary>
    /// 计算字符串像素长度
    /// </summary>
    /// <param name="message">文本内容</param>
    /// <param name="tex">要渲染这个文本的字体组件</param>
    /// <returns></returns>
    public static int CalcLengthOfText(this string message, Text tex)
    {
        int totalLength = 0;
        Font myFont = tex.font;
        myFont.RequestCharactersInTexture(message, tex.fontSize, tex.fontStyle);
        CharacterInfo characterInfo = new CharacterInfo();
        char[] arr = message.ToCharArray();
        foreach (char c in arr)
        {
            myFont.GetCharacterInfo(c, out characterInfo, tex.fontSize, tex.fontStyle);
            totalLength += characterInfo.advance;
        }

        return totalLength;
    }

    /// <summary>
    /// 首字母大写
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string UpperFirst(this string str)
    {
        return char.ToUpper(str[0]) + str.Substring(1);
    }

    /// <summary>
    /// 首字母小写
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string LowerFirst(this string str)
    {
        return char.ToLower(str[0]) + str.Substring(1);
    }

    /// <summary>
    /// 是否存在空格
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool HasSpace(this string input)
    {
        return input.Contains(" ");
    }

    /// <summary>
    /// 是否存在中文字符
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool HasChinese(this string input)
    {
        return Regex.IsMatch(input, @"[\u4e00-\u9fa5]");
    }

    #region 字符串转其他类型

    /// <summary>
    /// 字符串转整形
    /// </summary>
    /// <param name="selfStr">字符串</param>
    /// <param name="defaulValue">默认值</param>
    /// <returns></returns>
    public static int ToInt(this string selfStr, int defaulValue = 0)
    {
        var retValue = defaulValue;
        return int.TryParse(selfStr, out retValue) ? retValue : defaulValue;
    }

    /// <summary>
    /// 字符串转小数，兼容不同区域小数表示不一样的情况
    /// </summary>
    /// <param name="selfStr">字符串</param>
    /// <param name="defaulValue">默认值</param>
    /// <returns></returns>
    public static float ToFloat(this string selfStr, float defaulValue = 0)
    {
        //如果字符串为空 则返回默认值
        if (string.IsNullOrEmpty(selfStr)) return defaulValue;

        CultureInfo culture = Thread.CurrentThread.CurrentCulture;
        if (selfStr.Contains("."))
        {
            return float.Parse(selfStr.Replace(".", culture.NumberFormat.NumberDecimalSeparator));
        }
        else
        {
            return float.Parse(selfStr);
        }
    }

    /// <summary>
    /// 字符串转布尔
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool ToBool(this string selfStr)
    {
        selfStr = selfStr.Trim();
        return selfStr == "1" || selfStr == "是" || selfStr == "能" || selfStr == "可以" || selfStr == "行" ||
               selfStr == "yes" || selfStr == "Yes" || selfStr == "YES" || selfStr == "true" || selfStr == "True" ||
               selfStr == "TRUE";
    }

    /// <summary>
    /// 字符串转二维向量
    /// </summary>
    /// <param name="selfStr">字符串</param>
    /// <param name="defaultValue">默认值</param>
    /// <param name="safe">安全模式，安全模式会校验一下字符串，去除掉括号等用不到的符号</param>
    /// <returns></returns>
    public static Vector2 ToVector2(this string selfStr, Vector2 defaultValue = default, bool safe = true)
    {
        if (string.IsNullOrEmpty(selfStr)) return defaultValue;

        //去除掉一些用不到的符号
        if (safe)
        {
            selfStr = selfStr.Trim().Replace("(", "").Replace("（", "").Replace(")", "").Replace("）", "")
                .Replace("[", "").Replace("]", "");
        }

        //分割字符串 然后转换成向量
        char c = selfStr.Contains("，") ? '，' : selfStr.Contains(",") ? ',' : '|';
        string[] strs = selfStr.Split(c);
        return new Vector2(strs[0].ToFloat(), strs[1].ToFloat());
    }

    /// <summary>
    /// 字符串转三维向量
    /// </summary>
    /// <param name="selfStr">字符串</param>
    /// <param name="defaultValue">默认值</param>
    /// <param name="safe">安全模式，安全模式会校验一下字符串，去除掉括号等用不到的符号</param>
    /// <returns></returns>
    public static Vector3 ToVector3(this string selfStr, Vector3 defaultValue = default, bool safe = true)
    {
        if (string.IsNullOrEmpty(selfStr)) return defaultValue;

        //去除掉一些用不到的符号
        if (safe)
        {
            selfStr = selfStr.Trim().Replace("(", "").Replace("（", "").Replace(")", "").Replace("）", "")
                .Replace("[", "").Replace("]", "");
        }

        //分割字符串 然后转换成向量
        char c = selfStr.Contains("，") ? '，' : selfStr.Contains(",") ? ',' : '|';
        string[] strs = selfStr.Split(c);
        return new Vector3(strs[0].ToFloat(), strs[1].ToFloat(), strs.Length > 2 ? strs[2].ToFloat() : 0);
    }

    /// <summary>
    /// 字符串转四维向量
    /// </summary>
    /// <param name="selfStr">字符串</param>
    /// <param name="defaultValue">默认值</param>
    /// <param name="safe">安全模式，安全模式会校验一下字符串，去除掉括号等用不到的符号</param>
    /// <returns></returns>
    public static Vector4 ToVector4(this string selfStr, Vector4 defaultValue = default, bool safe = true)
    {
        if (string.IsNullOrEmpty(selfStr)) return defaultValue;

        //去除掉一些用不到的符号
        if (safe)
        {
            selfStr = selfStr.Trim().Replace("(", "").Replace("（", "").Replace(")", "").Replace("）", "")
                .Replace("[", "").Replace("]", "");
        }

        //分割字符串 然后转换成向量
        char c = selfStr.Contains("，") ? '，' : selfStr.Contains(",") ? ',' : '|';
        string[] strs = selfStr.Split(c);
        return new Vector4(strs[0].ToFloat(), strs[1].ToFloat(), strs.Length > 2 ? strs[2].ToFloat() : 0,
            strs.Length > 3 ? strs[3].ToFloat() : 0);
    }

    /// <summary>
    /// 字符串转颜色
    /// </summary>
    /// <param name="selfStr">字符串</param>
    /// <param name="defaultValue">默认值</param>
    /// <param name="safe">安全模式，安全模式会校验一下字符串，去除掉括号等用不到的符号</param>
    /// <returns></returns>
    public static Color ToColor(this string selfStr, Color defaultValue, bool safe = true)
    {
        if (string.IsNullOrEmpty(selfStr)) return defaultValue;

        //先将空格 以及 可能存在的一些辅助用的标识符号去掉
        if (safe)
        {
            selfStr = selfStr.Trim().Replace("(", "").Replace("（", "").Replace(")", "").Replace("）", "")
                .Replace("[", "").Replace("]", "").Replace("RGBA", "");
        }

        Color color = new Color();
        //不包含#或者~的 说明传入的是一个四维向量的字符串
        if (!selfStr.StartsWith("#") && !selfStr.StartsWith("~"))
        {
            char c = selfStr.Contains("，") ? '，' : selfStr.Contains(",") ? ',' : '|';
            string[] strs = selfStr.Split(c);
            color.r = strs[0].ToFloat();
            color.g = strs[1].ToFloat();
            color.b = strs[2].ToFloat();
            color.a = strs[3].ToFloat();
            return color;
        }

        //# 号在当前框架里 作为CSV文件的换行分割符 因此在表格里用~号代替#号，这里需要将其替换回来
        ColorUtility.TryParseHtmlString(selfStr.Replace("~", "#"), out color);
        return color;
    }

    #endregion

    /// <summary>
    /// 添加前缀
    /// </summary>
    /// <param name="selfStr">原字符串</param>
    /// <param name="toPrefix">前缀</param>
    /// <returns></returns>
    public static string AddPrefix(this string selfStr, string toPrefix)
    {
        return new StringBuilder(toPrefix).Append(selfStr).ToString();
    }

    /// <summary>
    /// 字符串转日期
    /// </summary>
    /// <param name="selfStr">字符串</param>
    /// <param name="defaultValue">默认日期</param>
    /// <returns></returns>
    public static DateTime ToDateTime(this string selfStr, DateTime defaultValue = default)
    {
        var retValue = defaultValue;
        return DateTime.TryParse(selfStr, out retValue) ? retValue : defaultValue;
    }

    /// <summary>
    /// 字符串里是否包含条件判断
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool ContainsJudge(this string str)
    {
        return str.Contains("大于") || str.Contains(">") ||
               str.Contains("小于") || str.Contains("<") ||
               str.Contains("大于等于") || str.Contains(">=") ||
               str.Contains("小于等于") || str.Contains("<=") ||
               str.Contains("等于") || str.Contains("=") || str.Contains("==") ||
               str.Contains("不等于") || str.Contains("!=") ||
               str.Contains("包含") || str.Contains("不包含") ||
               str.Contains("含于") || str.Contains("不含于");
    }

    /// <summary>
    /// 分割字符串[[[]]]，[[[]]]，[[]]，[]，[[]]
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string[] SplitEventStr(this string content)
    {
        //如果不包含括号的话 说明不需要分割 直接返回
        if (!content.Contains("["))
        {
            return new string[] { content };
        }

        List<string> list = new List<string>();
        int symbolCount = 0;
        int oldIndex = 0;
        for (int i = 0; i < content.Length; i++)
        {
            //左括号+1 右括号-1 一左一右抵消后就为零
            if (content[i] == '[')
            {
                symbolCount++;
            }

            if (content[i] == ']')
            {
                symbolCount--;
            }

            //如果括号数量为零了 那这之间的字符串就是一块内容
            if (symbolCount == 0)
            {
                string str = content.Substring(oldIndex + 1, i - oldIndex - 1);
                list.Add(str);
                //因为[],[]  []，[] 间有一个分割用的字符 因此这里需要额外再让i+1 让他直接从下一个 [ 开始
                i += 1;
                // ]，[ 右括号是当前当前i的位置是逗号 让他再+1 之后 位置就来到了左括号这里 也就是下一块内容的起点
                oldIndex = i + 1;
            }
        }

        return list.ToArray();
    }
}