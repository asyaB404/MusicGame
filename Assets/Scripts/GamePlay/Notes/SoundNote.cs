namespace GamePlay.Notes
{
    /// <summary>
    /// 音效类Note，自动处理音符，用于打节拍提示用
    /// </summary>
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