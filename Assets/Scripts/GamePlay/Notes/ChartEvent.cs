namespace GamePlay.Notes
{
    [System.Serializable]
    public abstract class ChartEvent : Note
    {
        protected ChartEvent(float beat) : base(beat, true)
        {
        }
    }
}