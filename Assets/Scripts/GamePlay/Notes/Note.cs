namespace GamePlay.Notes
{
    [System.Serializable]
    public abstract class Note
    {
        public int id;
        public float beat;
        public int pos;
        public bool auto;

        protected Note(float beat, int pos)
        {
            this.beat = beat;
            this.pos = pos;
        }
        
        protected Note(float beat, int pos, bool auto)
        {
            this.beat = beat;
            this.pos = pos;
            this.auto = auto;
        }

        protected Note()
        {
            throw new System.NotImplementedException();
        }
    }
}