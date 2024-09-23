namespace GamePlay.Notes
{
    public enum SoundType
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Tap,
        Tick
    }

    [System.Serializable]
    public class SoundNote : Note
    {
        public SoundType soundType;

        public SoundNote(float beat, int pos, SoundType soundType) : base(beat, pos, true)
        {
            this.soundType = soundType;
        }
    }
}