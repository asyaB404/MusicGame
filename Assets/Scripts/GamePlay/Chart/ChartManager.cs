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
    private void TestStart()
    {
        chart = Chart.SampleChart();
        GameManager.Instance.StartGame();
    }
    
    [ContextMenu("sampleStart2")]
    private void TestStart2()
    {
        chart = Chart.SampleChart2();
        GameManager.Instance.StartGame();
    }

    #endregion
}