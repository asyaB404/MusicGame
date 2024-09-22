namespace GamePlay.Notes
{
    [System.Serializable]
    public abstract class Note
    {
        public float beat;
        public int pos;

        protected Note(float beat, int pos)
        {
            this.beat = beat;
            this.pos = pos;
        }

        protected Note()
        {
            throw new System.NotImplementedException();
        }
    }
}