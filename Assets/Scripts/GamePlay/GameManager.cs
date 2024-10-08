using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GamePlay.Notes;
using UnityEngine;


namespace GamePlay
{
    /// <summary>
    /// 负责管理游戏状态和音符判定队列
    /// </summary>
    public class GameManager : MonoManager<GameManager>
    {
        /// <summary>
        /// 表示轨道数量
        /// </summary>
        public static int KeysCount { get; private set; } = 8;

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
        public static bool IsStart => Instance.isStart;

        [SerializeField] private int combo;

        /// <summary>
        /// 歌曲bpm
        /// </summary>
        [SerializeField] private float bpm;

        public static float Bpm => Instance.bpm;


        /// <summary>
        /// 当前位于的节拍
        /// </summary>
        [SerializeField] private float curBeat;

        public static float CurBeat => Instance.curBeat;

        /// <summary>
        /// 计时器，记录当前的时间
        /// </summary>
        [SerializeField] private float timer;

        /// <summary>
        /// 从0开始，每个轨道将要加入的音符下标，添加的速度和范围取决于noteMissRange
        /// </summary>
        [SerializeField] private List<int> curNotesIndexList;

        /// <summary>
        /// 可以被判定的Note队列组
        /// </summary>
        private List<Queue<Note>> _canHitNotesQueues;


        public void Start()
        {
            // 不知道为什么手机锁30帧，加了这个能改善，但是锁60
            Application.targetFrameRate = 240;
        }

        private void Update()
        {
            if (!isStart) return;
            timer += Time.deltaTime;
            // 更新当前的节拍数
            curBeat = timer * (bpm / 60f);

            // 遍历每个轨道的音符队列
            for (int pos = 0; pos < _canHitNotesQueues.Count; pos++)
            {
                // 将已经进入判定区的音符加入队列
                while (curNotesIndexList[pos] < ChartManager.CurChart.notes[pos].Count &&
                       curBeat + noteMissRange * (bpm / 60f) >=
                       ChartManager.CurChart.notes[pos][curNotesIndexList[pos]].beat)
                {
                    Note curChartNote = ChartManager.CurChart.notes[pos][curNotesIndexList[pos]];
                    curChartNote.id = curNotesIndexList[pos];
                    if (curChartNote.auto)
                    {
                        // 处理自动播放音符
                        ResolveAutoNote(curChartNote, 0, curChartNote.beat - curBeat);
                    }
                    else
                    {
                        // 将音符加入可判定队列
                        _canHitNotesQueues[pos].Enqueue(ChartManager.CurChart.notes[pos][curNotesIndexList[pos]]);
                    }

                    // 更新音符下标
                    curNotesIndexList[pos]++;
                }

                // 检查可打击的音符中是否有Miss，有则移除
                while (_canHitNotesQueues[pos].Count > 0 &&
                       _canHitNotesQueues[pos].Peek().beat + noteMissRange * (bpm / 60f) <= curBeat)
                {
                    _canHitNotesQueues[pos].Dequeue();
                }
            }

            // 若当前节拍已超过谱面总节拍，则重置游戏
            if (curBeat >= ChartManager.CurChart.totalBeat)
            {
                StateReset();
            }
        }

        /// <summary>
        /// 开始播放某个谱面
        /// </summary>
        [ContextMenu("开始")]
        public void StartGame()
        {
            bpm = ChartManager.CurChart.curBpm;
            KeysCount = ChartManager.CurChart.keys;
            StateReset();
            isStart = true;
        }

        [ContextMenu("重置")]
        public void StateReset()
        {
            isStart = false;
            timer = 0;
            curBeat = 0;
            curNotesIndexList = new List<int>(KeysCount);
            _canHitNotesQueues = new List<Queue<Note>>(KeysCount);
            for (int i = 0; i < KeysCount; i++)
            {
                curNotesIndexList.Add(0);
                _canHitNotesQueues.Add(new Queue<Note>());
            }

            NotesObjManager.Instance.StateReset();
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
                case TapNote tapNote:
                    NotesObjManager.Instance.PlayEffect(pos, 0);
                    break;
                case SoundNote soundNote:
                    Debug.Log("位于轨道" + pos + "的" + soundNote.soundType);
                    break;
                case ChangeBpmNote changeBpmNote:
                    bpm = changeBpmNote.targetBpm;
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
        public void ResolveNote(int pos = 0)
        {
            if (pos == -1) return;
            if (_canHitNotesQueues?[pos] == null ||
                _canHitNotesQueues[pos].Count <= 0) return;
            Note note = _canHitNotesQueues[pos].Dequeue();
            NotesObjManager.Instance.ResolveNote(note, pos);
            var abs = Mathf.Abs(curBeat - note.beat) * (60f / bpm);
            Debug.Log((curBeat - note.beat) * (60f / bpm));
            switch (note)
            {
                case TapNote:
                    if (abs <= notePrefectRange)
                    {
                        NotesObjManager.Instance.PlayEffect(pos, 0);
                    }
                    else if (abs <= noteGreatRange)
                    {
                        NotesObjManager.Instance.PlayEffect(pos, 1);
                    }
                    else
                    {
                        Debug.Log("click_miss");
                    }

                    break;
            }
        }
    }
}