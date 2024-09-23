using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GamePlay.Notes;
using UnityEngine;


namespace GamePlay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float noteGreatRange;
        [SerializeField] private float notePrefectRange;
        [SerializeField] private float noteMissRange;
        [SerializeField] private bool isStart;
        [SerializeField] private int combo;
        [SerializeField] private float bpm;
        [SerializeField] private float curBeat;
        [SerializeField] private float timer;
        public Chart curChart;
        [SerializeField] private int notesIndex;
        private Queue<Note> _canHitNotesQueue;
        public IReadOnlyCollection<Note> CanHitNotesQueue => _canHitNotesQueue;

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
                   curBeat + noteMissRange * (bpm / 60) >= curChart.notes[notesIndex].beat)
            {
                Note curChartNote = curChart.notes[notesIndex];
                if (curChartNote.auto)
                {
                    ResolveAutoNote(curChartNote, curChartNote.beat - curBeat);
                }
                else
                {
                    _canHitNotesQueue.Enqueue(curChart.notes[notesIndex]);
                }

                notesIndex++;
            }

            while (_canHitNotesQueue.Count > 0 &&
                   _canHitNotesQueue.Peek().beat + noteMissRange * (bpm / 60) <= curBeat)
            {
                Debug.Log("miss");
                _canHitNotesQueue.Dequeue();
            }

            if (curBeat >= curChart.totalBeat)
            {
                Reset();
            }
        }

        public void Hit(Touch touch)
        {
        }

        private void StartChart(Chart chart)
        {
            isStart = true;
        }

        private void Reset()
        {
            isStart = false;
            timer = 0;
            curBeat = 0;
            notesIndex = 0;
        }

        private async void ResolveAutoNote(Note note, float delay = 0)
        {
            if (delay > 0)
                await UniTask.Delay(System.TimeSpan.FromSeconds(delay));

            switch (note)
            {
                case SoundNote:
                    break;
                case ChangeBpmNote:
                    break;
                default:
                    Debug.LogError("音符类型错误！");
                    break;
            }
        }

        private async void ResolveNote(float delay = 0)
        {
            if (delay > 0)
                await UniTask.Delay(System.TimeSpan.FromSeconds(delay));
            Note note = _canHitNotesQueue.Peek();
            switch (note)
            {
                case TapNote:
                    if (Mathf.Abs(curBeat - note.beat) * (60 / bpm) <= notePrefectRange)
                    {
                    }
                    else if (Mathf.Abs(curBeat - note.beat) * (60 / bpm) <= noteGreatRange)
                    {
                    }
                    else
                    {
                        Debug.Log("click_miss");
                    }

                    break;
            }

            Note dequeue = _canHitNotesQueue.Dequeue();
            // if (dequeue != note)
            // {
            //     Debug.LogError("怎么回事");
            // }
        }
    }
}