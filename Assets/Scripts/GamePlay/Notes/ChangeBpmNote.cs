namespace GamePlay.Notes
{
    [System.Serializable]
    public class ChangeBpmNote : Note
    {
        public float targetBpm;

        public ChangeBpmNote(float beat, int pos, float targetBpm) : base(beat, pos, true)
        {
            this.targetBpm = targetBpm;
        }
    }
}