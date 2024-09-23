using System;
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

        [Space(20)] [Header("----------分割线----------")] [SerializeField]
        private float musicLength;

        [SerializeField] private bool isStart;
        [SerializeField] private float curBeat;
        [SerializeField] private float timer;
        [SerializeField] private float totalBeat;
        [SerializeField] private float curBpm;
        [SerializeField] private Chart curChart;
        public List<Note> Notes = new();

        [ContextMenu("导入歌曲并新建谱面")]
        public void CreateNewChart()
        {
            musicLength = music.length;
            curChart.musicLength = musicLength;
            curChart.curBpm = bpm;
            curChart.totalBeat = musicLength * (bpm / 60);
            curChart.offset = offset;
        }

        private void Update()
        {
            if (!isStart) return;
            timer += Time.deltaTime;
            curBeat = timer * (bpm / 60);
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