namespace GamePlay.Notes
{
    /// <summary>
    /// 音符基类
    /// </summary>
    [System.Serializable]
    public abstract class Note
    {
        /// <summary>
        /// id，在游戏中才赋值
        /// </summary>
        public int id;

        /// <summary>
        /// 位于谱面的节拍处
        /// </summary>
        public float beat;

        /// <summary>
        /// 是否是自动处理的音符（是否加入玩家手动判定区间队列
        /// </summary>
        public bool auto;

        protected Note(float beat)
        {
            this.beat = beat;
        }

        protected Note(float beat, bool auto)
        {
            this.beat = beat;
            this.auto = auto;
        }

        protected Note()
        {
            throw new System.NotImplementedException();
        }
    }
}