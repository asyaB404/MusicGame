namespace GamePlay.Notes
{
    [System.Serializable]
    public class SoundNote : Note
    {
        public SoundType soundType;

        public SoundNote(float beat, int pos) : base(beat, pos, true)
        {
            this.soundType = SoundType.Tick;
        }

        public SoundNote(float beat, int pos, SoundType soundType) : base(beat, pos, true)
        {
            this.soundType = soundType;
        }

        public SoundNote()
        {
            throw new System.NotImplementedException();
        }
    }
}