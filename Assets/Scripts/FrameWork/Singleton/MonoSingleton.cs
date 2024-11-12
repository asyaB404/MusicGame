using UnityEngine;
/// <summary>
/// Mono类单例模板(大概率没必要像这个脚本一样写的那么复杂，随便创建个实例来用即可)
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Instance = null;
    private static readonly object m_Lock = new object();
    public static T Instance
    {
        get
        {
            //防止多线程时的数据冲突
            lock (m_Lock)
            {
                //如果单例对象存在 就直接返回出去
                if (m_Instance != null)
                {
                    return m_Instance;
                }

                //场景里找
                m_Instance = FindObjectOfType<T>();
                if (m_Instance != null)
                {
                    return m_Instance;
                }

                //创建个新的
                var obj = new GameObject(typeof(T).ToString());
                m_Instance = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);
                return m_Instance;
            }
        }
    }

    public void OnDestroy()
    {
        m_Instance = null;
    }
}