using System.Collections.Generic;
using GamePlay.Notes;
using UnityEngine;
using UnityEngine.Serialization;

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

        [FormerlySerializedAs("curChart")] [SerializeField]
        private ChartInfo curChartInfo;

        public List<Note> Notes = new();

        [ContextMenu("导入歌曲并新建谱面")]
        public void CreateNewChart()
        {
            musicLength = music.length;
            curChartInfo.musicLength = musicLength;
            curChartInfo.curBpm = bpm;
            curChartInfo.totalBeat = musicLength * (bpm / 60);
            curChartInfo.offset = offset;
        }

        private void Update()
        {
            if (!isStart) return;
            timer += Time.deltaTime;
            curBeat = timer * (bpm / 60);
        }

        public void AddNote(int pos, Note note)
        {
            curChartInfo.notes[pos].Add(note);
        }

        public ChartInfo ExportChart()
        {
            for (int i = 0; i < curChartInfo.notes.Count; i++)
            {
                curChartInfo.notes[i].Sort((note1, note2) => note1.beat.CompareTo(note2.beat));
            }

            return curChartInfo;
        }
    }
}