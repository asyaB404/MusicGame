using System.Collections.Generic;
using GamePlay.Notes;
using UnityEngine;

namespace CustomChartEditor
{
    public class CustomChartManager : MonoBehaviour
    {
        [SerializeField] private float offset;
        [SerializeField] private float bpm;
        [SerializeField] private AudioClip music;
        [SerializeField] private float musicLength;
        public float totalBeat;
        public float curBpm;
        public List<Note> Notes = new();
        public Chart curChart;

        [ContextMenu("导入歌曲并新建谱面")]
        public void CreateNewChart()
        {
            musicLength = music.length;
            curChart.musicLength = musicLength;
            curChart.curBpm = bpm;
            curChart.totalBeat = musicLength * (bpm / 60);
            curChart.offset = offset;
        }

        public void AddNote(Note note)
        {
            curChart.notes.Add(note);
        }

        public Chart ExportChart()
        {
            curChart.notes.Sort((note1, note2) => note1.beat.CompareTo(note2.beat));
            return curChart;
        }
    }
}