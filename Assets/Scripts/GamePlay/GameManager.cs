using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GamePlay.Notes;
using UnityEngine;


namespace GamePlay
{
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// 表示轨道数量
        /// </summary>
        public static int KeysCount = 2;

        /// <summary>
        /// Miss区间，音符进入该区间后将可进行判定（其实就是加入相应轨道的判定队列
        /// </summary>
        [SerializeField] private float noteMissRange;

        /// <summary>
        /// Great判定±区间
        /// </summary>
        [SerializeField] private float noteGreatRange;

        /// <summary>
        /// Prefect判定±区间
        /// </summary>
        [SerializeField] private float notePrefectRange;

        [SerializeField] private bool isStart;
        [SerializeField] private int combo;

        /// <summary>
        /// 歌曲bpm
        /// </summary>
        [SerializeField] private float bpm;

        /// <summary>
        /// 当前位于的节拍
        /// </summary>
        [SerializeField] private float curBeat;

        /// <summary>
        /// 计时器，记录当前的时间
        /// </summary>
        [SerializeField] private float timer;

        /// <summary>
        /// 当前的谱面
        /// </summary>
        public Chart curChart;

        /// <summary>
        /// 从0开始，每个轨道将要加入的音符下标
        /// </summary>
        /// <example>例如一个谱面100个音符且平均分配时，歌曲处于一半的时候，该下标为50</example>
        [SerializeField] private List<int> curNotesIndexList = new(KeysCount);

        /// <summary>
        /// 可以被判定的Note队列组
        /// </summary>
        private readonly List<Queue<Note>> _canHitNotesQueues = new(KeysCount);

        /// <summary>
        /// 单例
        /// </summary>
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            // 初始化轨道列表，确保每个轨道都有一个初始索引和队列
            for (int i = 0; i < KeysCount; i++)
            {
                curNotesIndexList.Add(0);
                _canHitNotesQueues.Add(new Queue<Note>());
            }
        }

        public void Start()
        {
            // 不知道为什么手机锁30帧，加了这个能改善，但是锁60
            Application.targetFrameRate = 120;
        }

        private void Update()
        {
            if (!isStart) return;
            timer += Time.deltaTime;
            // 更新当前的节拍数
            curBeat = timer * (bpm / 60);

            // 遍历每个轨道的音符队列
            for (int pos = 0; pos < _canHitNotesQueues.Count; pos++)
            {
                // 将已经进入判定区的音符加入队列
                while (curNotesIndexList[pos] < curChart.notes[pos].Count &&
                       curBeat + noteMissRange * (bpm / 60) >= curChart.notes[pos][curNotesIndexList[pos]].beat)
                {
                    Note curChartNote = curChart.notes[pos][curNotesIndexList[pos]];
                    if (curChartNote.auto)
                    {
                        // 处理自动播放音符
                        ResolveAutoNote(curChartNote, 0, curChartNote.beat - curBeat);
                    }
                    else
                    {
                        // 将音符加入可判定队列
                        _canHitNotesQueues[pos].Enqueue(curChart.notes[pos][curNotesIndexList[pos]]);
                    }
                    
                    // 更新音符下标
                    curNotesIndexList[pos]++;
                }

                // 检查可打击的音符中是否有Miss，有则移除
                while (_canHitNotesQueues[pos].Count > 0 &&
                       _canHitNotesQueues[pos].Peek().beat + noteMissRange * (bpm / 60) <= curBeat)
                {
                    Debug.Log("miss");
                    _canHitNotesQueues[pos].Dequeue();
                }
            }

            // 若当前节拍已超过谱面总节拍，则重置游戏
            if (curBeat >= curChart.totalBeat)
            {
                Reset();
            }
        }

        /// <summary>
        /// 开始播放某个谱面
        /// </summary>
        /// <param name="chart">要播放的谱面</param>
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

        /// <summary>
        /// 处理自动音符
        /// </summary>
        /// <param name="note">当前音符</param>
        /// <param name="pos">轨道位置</param>
        /// <param name="delay">延迟执行时间</param>
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

        /// <summary>
        /// 手动击打音符的判定逻辑，优先处理队列首位的音符
        /// </summary>
        /// <param name="pos">轨道位置</param>
        /// <param name="delay">延迟执行时间</param>
        public async void ResolveNote(int pos = 0, float delay = 0)
        {
            if (_canHitNotesQueues[pos].Count <= 0) return;
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