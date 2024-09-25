using UnityEngine;

public class ChartManager : MonoBehaviour
{
    /// <summary>
    /// 表示轨道数量
    /// </summary>
    public static int KeysCount { get; private set; } = 2;

    public static ChartManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}