namespace GamePlay.Notes
{
    /// <summary>
    /// 长按音符，头判算一次判定，接下来持续按住算一次判定。长按中断了可以接回去
    /// 参考maimai的hold按键
    /// </summary>
    [System.Serializable]
    public class HoldNote : Note
    {
        public float endBeat;

        public HoldNote(float beat, float endBeat) : base(beat)
        {
            this.endBeat = endBeat;
        }

        public HoldNote(float beat, bool auto, float endBeat) : base(beat, auto)
        {
            this.endBeat = endBeat;
        }

        public HoldNote(float endBeat)
        {
            this.endBeat = endBeat;
        }
    }
}