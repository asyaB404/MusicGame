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

    #endregion
}