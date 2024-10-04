namespace GamePlay.Notes
{
    [System.Serializable]
    public class ChangeBpmNote : ChartEvent
    {
        public float targetBpm;

        public ChangeBpmNote(float beat, int pos, float targetBpm) : base(beat, pos)
        {
            this.targetBpm = targetBpm;
        }
    }
}