namespace GamePlay.Notes
{
    [System.Serializable]
    public class TapNote : Note
    {
        public TapNote(float beat, int pos) : base(beat, pos)
        {
        }

        public TapNote(float beat, int pos, bool auto) : base(beat, pos, auto)
        {
        }
    }
}