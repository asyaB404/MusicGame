namespace GamePlay.Notes
{
    public enum SoundType
    {
        Tick,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Tap,
        
    }

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