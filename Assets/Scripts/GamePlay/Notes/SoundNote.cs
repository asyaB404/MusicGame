namespace GamePlay.Notes
{
    [System.Serializable]
    public class SoundNote : Note
    {
        public SoundType soundType;

        public SoundNote(float beat) : base(beat, true)
        {
            this.soundType = SoundType.Tick;
        }

        public SoundNote(float beat, SoundType soundType) : base(beat, true)
        {
            this.soundType = soundType;
        }

        public SoundNote()
        {
            throw new System.NotImplementedException();
        }
    }
}