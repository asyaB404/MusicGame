using System.Collections.Generic;
using GamePlay.Notes;


[System.Serializable]
public class Chart
{
    public int keys;
    public float offset;
    public float musicLength;
    public float totalBeat;
    public float curBpm;
    public List<List<Note>> notes = new();


    public static Chart SampleChart()
    {
        Chart chart = new Chart
        {
            keys = 8,
            curBpm = 90,
            musicLength = 30f,
            totalBeat = 60f,
            notes = new List<List<Note>>() { new(), new(), new(), new(), new(), new(), new(), new() }
        };
        List<Note> notes0 = new List<Note>() { };
        AddTap7_th(notes0, 6);
        AddTap7_th(notes0, 6 + 7);
        AddTap7_th(notes0, 6 + 7 * 2);
        AddTap7_th(notes0, 6 + 7 * 3);
        AddTap7_th(notes0, 6 + 7 * 4, 0.5f);
        AddTap7_th(notes0, 6 + 7 * 4.5f, 0.5f);
        chart.notes[0] = notes0;
        return chart;
    }

    public static Chart SampleChart2()
    {
        Chart chart = new Chart
        {
            keys = 8,
            curBpm = 90,
            musicLength = 30f,
            totalBeat = 60f,
            notes = new List<List<Note>>() { new(), new(), new(), new(), new(), new(), new(), new() }
        };
        List<Note> notes0 = new List<Note>() { };
        List<Note> notes1 = new List<Note>() { };
        List<Note> notes2 = new List<Note>() { };
        List<Note> notes3 = new List<Note>() { };
        List<Note> notes4 = new List<Note>() { };
        List<Note> notes5 = new List<Note>() { };
        List<Note> notes6 = new List<Note>() { };
        List<Note> notes7 = new List<Note>() { };
        AddTap7_th(notes0, 6);
        AddTap7_th(notes1, 6 + 7);
        AddTap7_th(notes2, 6 + 7 * 2);
        AddTap7_th(notes3, 6 + 7 * 3);
        AddTap7_th(notes4, 6 + 7 * 4, 0.5f);
        AddTap7_th(notes5, 6 + 7 * 5f);
        AddTap7_th(notes6, 6 + 7 * 6f, 0.5f);
        AddTap7_th(notes7, 6 + 7 * 7f);
        chart.notes = new() { notes0, notes1, notes2, notes3, notes4, notes5, notes6, notes7 };
        return chart;
    }

    private static void AddTap7_th(IList<Note> notes, float start = 0, float duration = 1)
    {
        for (int i = 0; i < 6; i++)
        {
            notes.Add(new TapNote(start + duration * i));
        }

        notes.Add(new TapNote(start + duration * 6));
    }
}