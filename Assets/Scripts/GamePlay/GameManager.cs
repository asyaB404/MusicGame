using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GamePlay.Notes;
using UnityEngine;
using UnityEngine.Serialization;


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
        [SerializeField] private List<int> curNotesIndexList = new() { 0, 0 };
        private readonly List<Queue<Note>> _canHitNotesQueues = new() { new Queue<Note>(), new Queue<Note>() };

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            Application.targetFrameRate = 120;
        }

        private void Update()
        {
            if (!isStart) return;
            timer += Time.deltaTime;
            curBeat = timer * (bpm / 60);
            for (int i = 0; i < _canHitNotesQueues.Count; i++)
            {
                while (curNotesIndexList[i] < curChart.notes.Count &&
                       curBeat + noteMissRange * (bpm / 60) >= curChart.notes[curNotesIndexList[i]].beat)
                {
                    Note curChartNote = curChart.notes[curNotesIndexList[i]];
                    if (curChartNote.auto)
                    {
                        ResolveAutoNote(curChartNote, 0, curChartNote.beat - curBeat);
                    }
                    else
                    {
                        _canHitNotesQueues[i].Enqueue(curChart.notes[curNotesIndexList[i]]);
                    }

                    curNotesIndexList[i]++;
                }

                while (_canHitNotesQueues.Count > 0 &&
                       _canHitNotesQueues[i].Peek().beat + noteMissRange * (bpm / 60) <= curBeat)
                {
                    Debug.Log("miss");
                    _canHitNotesQueues[i].Dequeue();
                }
            }


            if (curBeat >= curChart.totalBeat)
            {
                Reset();
            }
        }

        private void StartChart(Chart chart)
        {
            curChart = chart;
            bpm = chart.curBpm;
            isStart = true;
        }

        [ContextMenu("重置")]
        private void Reset()
        {
            isStart = false;
            timer = 0;
            curBeat = 0;
            curNotesIndexList = new List<int>() { 0, 0 };
        }

        public async void ResolveAutoNote(Note note, int pos = 0, float delay = 0)
        {
            if (delay > 0)
                await UniTask.Delay(System.TimeSpan.FromSeconds(delay));

            switch (note)
            {
                case SoundNote soundNote:
                    Debug.Log(soundNote.soundType);
                    break;
                case ChangeBpmNote:
                    break;
                default:
                    Debug.LogError("音符类型错误！");
                    break;
            }
        }

        public async void ResolveNote(int pos = 0, float delay = 0)
        {
            if (_canHitNotesQueues.Count <= 0) return;
            if (delay > 0)
                await UniTask.Delay(System.TimeSpan.FromSeconds(delay));
            Note note = _canHitNotesQueues[pos].Peek();
            switch (note)
            {
                case TapNote:
                    if (Mathf.Abs(curBeat - note.beat) * (60 / bpm) <= notePrefectRange)
                    {
                        Debug.Log("perfect");
                    }
                    else if (Mathf.Abs(curBeat - note.beat) * (60 / bpm) <= noteGreatRange)
                    {
                        Debug.Log("great");
                    }
                    else
                    {
                        Debug.Log("click_miss");
                    }

                    break;
            }

            Note dequeue = _canHitNotesQueues[pos].Dequeue();
            if (dequeue != note)
            {
                Debug.LogError("怎么回事");
            }
        }

        #region Debug

        [ContextMenu("sampleStart")]
        private void TestStart()
        {
            Reset();
            var sampleChart = Chart.SampleChart();
            StartChart(sampleChart);
        }

        #endregion
    }
}