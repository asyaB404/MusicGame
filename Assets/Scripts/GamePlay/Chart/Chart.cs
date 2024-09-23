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


    public static Chart SampleChart()
    {
        Chart chart = new Chart();
        chart.curBpm = 90;
        chart.musicLength = 30f;
        chart.totalBeat = 60f;
        List<Note> notes = new List<Note>();
        AddTap7_th(notes, 0, 6);
        AddTap7_th(notes, 0, 6 + 7);
        AddTap7_th(notes, 0, 6 + 7 * 2);
        AddTap7_th(notes, 0, 6 + 7 * 3);
        AddTap7_th(notes, 0, 6 + 7 * 4, 0.5f);
        AddTap7_th(notes, 0, 6 + 7 * 4.5f, 0.5f);
        chart.notes = notes;
        return chart;
    }

    private static void AddTap7_th(IList<Note> notes, int pos = 0, float start = 0, float duration = 1)
    {
        for (int i = 0; i < 6; i++)
        {
            notes.Add(new SoundNote(start + duration * i, pos));
        }

        notes.Add(new TapNote(start + duration * 6, pos));
    }
}