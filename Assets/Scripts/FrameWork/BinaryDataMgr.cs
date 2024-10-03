using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// 2进制数据管理器
/// </summary>
public class BinaryDataMgr
{
    /// <summary>
    /// 数据存储的位置
    /// </summary>
    private static readonly string SAVE_PATH = Application.persistentDataPath + "/SaveData/";

    private static readonly string EXTENSION_NAME = ".saveData";
    private static readonly BinaryDataMgr instance = new();
    public static BinaryDataMgr Instance => instance;

    /// <summary>
    /// 存储类对象数据
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="fileName"></param>
    public void Save(object obj, string fileName)
    {
        if (!Directory.Exists(SAVE_PATH))
            Directory.CreateDirectory(SAVE_PATH);

        using FileStream fs =
            new(SAVE_PATH + fileName + EXTENSION_NAME, FileMode.OpenOrCreate, FileAccess.Write);
        BinaryFormatter bf = new();
        bf.Serialize(fs, obj);
        fs.Close();
    }

    /// <summary>
    /// 读取2进制数据转换成对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public T Load<T>(string fileName)
        where T : class
    {
        //如果不存在这个文件 就直接返回泛型对象的默认值
        if (!File.Exists(SAVE_PATH + fileName + EXTENSION_NAME))
        {
            Debug.LogWarning("找不到位于" + SAVE_PATH + fileName + "存档");
            return default(T);
        }

        T obj;
        using (
            FileStream fs = File.Open(
                SAVE_PATH + fileName + EXTENSION_NAME,
                FileMode.Open,
                FileAccess.Read
            )
        )
        {
            BinaryFormatter bf = new();
            obj = bf.Deserialize(fs) as T;
            fs.Close();
        }

        return obj;
    }
}