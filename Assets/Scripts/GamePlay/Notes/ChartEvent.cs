namespace GamePlay.Notes
{
    /// <summary>
    /// 谱面事件类，本质也是Note，但不同的是没有对应的GameObject实体生成
    /// </summary>
    [System.Serializable]
    public abstract class ChartEvent : Note
    {
        protected ChartEvent(float beat) : base(beat, true)
        {
        }
    }
}