using System;
using System.Collections.Generic;
using GamePlay.Notes;
using UnityEngine;

namespace GamePlay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float canHitNoteRange;
        [SerializeField] private bool isStart;
        [SerializeField] private int combo;
        [SerializeField] private float bpm;
        [SerializeField] private float curBeat;
        [SerializeField] private float timer;
        public Chart curChart;
        [SerializeField] private int notesIndex;
        private Queue<Note> _notesQueue;
        public IReadOnlyCollection<Note> NotesQueue => _notesQueue;

        public void Start()
        {
            Application.targetFrameRate = 120;
        }

        private void Update()
        {
            if (!isStart) return;
            timer += Time.deltaTime;
            curBeat = timer * (bpm / 60);
            while (notesIndex < curChart.notes.Count &&
                   curBeat + canHitNoteRange * (bpm / 60) >= curChart.notes[notesIndex].beat)
            {
                _notesQueue.Enqueue(curChart.notes[notesIndex]);
                notesIndex++;
            }

            while (notesIndex < curChart.notes.Count &&
                   curBeat + canHitNoteRange * (bpm / 60) < _notesQueue.Peek().beat)
            {
                Debug.Log("miss");
                _notesQueue.Dequeue();
            }
        }

        private void StartChart(Chart chart)
        {
            isStart = true;
        }

        private void Init()
        {
            timer = 0;
            curBeat = 0;
            notesIndex = 0;
        }

        private void ReStart()
        {
            timer = 0;
            curBeat = 0;
        }
    }
}