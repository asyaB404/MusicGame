namespace GamePlay.Notes
{
    [System.Serializable]
    public abstract class Note
    {
        public int id;
        public float beat;
        public bool auto;

        protected Note(float beat)
        {
            this.beat = beat;
        }
        
        protected Note(float beat,bool auto)
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