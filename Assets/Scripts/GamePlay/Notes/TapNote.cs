namespace GamePlay.Notes
{
    [System.Serializable]
    public class TapNote : Note
    {
        public TapNote(float beat) : base(beat)
        {
        }

        public TapNote(float beat, bool auto) : base(beat, auto)
        {
        }
    }
}