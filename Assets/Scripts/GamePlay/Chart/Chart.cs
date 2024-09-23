using System.Collections.Generic;
using GamePlay.Notes;

[System.Serializable]
public class Chart
{
    public float offset;
    public float musicLength;
    public float totalBeat;
    public float curBpm;
    public List<Note> notes;
}