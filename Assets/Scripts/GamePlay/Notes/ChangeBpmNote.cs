namespace GamePlay.Notes
{
    /// <summary>
    /// 变换Bpm事件
    /// </summary>
    [System.Serializable]
    public class ChangeBpmNote : ChartEvent
    {
        public float targetBpm;

        public ChangeBpmNote(float beat, float targetBpm) : base(beat)
        {
            this.targetBpm = targetBpm;
        }
    }
}