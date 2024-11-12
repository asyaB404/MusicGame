using GamePlay;
using UnityEngine;

public class ChartManager : MonoManager<ChartManager>
{
    public static Chart CurChart
    {
        get => Instance.chart;
        set => Instance.chart = value;
    }

    [SerializeField] private Chart chart;


    #region Debug

    [ContextMenu("sampleStart")]
    public void TestStart()
    {
        chart = Chart.SampleChart();
        GameManager.Instance.StartGame();
    }
    
    [ContextMenu("sampleStart2")]
    public void TestStart2()
    {
        chart = Chart.SampleChart2();
        GameManager.Instance.StartGame();
    }

    #endregion
}