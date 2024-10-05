namespace GamePlay.Notes
{
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