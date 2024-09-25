using GamePlay;
using UnityEngine;

public class ChartManager : MonoBehaviour
{
    /// <summary>
    /// 表示轨道数量
    /// </summary>
    public static int KeysCount { get; private set; } = 2;

    public static ChartManager Instance { get; private set; }
    public static Chart Chart => Instance.chart;

    [SerializeField] private Chart chart;

    private void Awake()
    {
        Instance = this;
    }

    #region Debug

    [ContextMenu("sampleStart")]
    private void TestStart()
    {
        GameManager.Instance.Reset();
        chart = Chart.SampleChart();
        GameManager.Instance.StartGame();
    }

    #endregion
}