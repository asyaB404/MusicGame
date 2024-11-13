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
        #region 字段

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
        public static float CurTimer => Instance.timer;

        /// <summary>
        /// 从0开始，每个轨道将要加入的音符下标，添加的速度和范围取决于noteMissRange
        /// </summary>
        [SerializeField] private List<int> curNotesIndexList;

        /// <summary>
        /// 可以被判定的Note队列组
        /// </summary>
        private List<Queue<Note>> _canHitTapNotesQueues;

        private List<Queue<(float startBeat, float endBeat, float totalHoldTimer)>> _canHoldNotesQueues;

        #endregion

        public async void Start()
        {
            Application.targetFrameRate = 9999;
            QualitySettings.vSyncCount = 0;
            await UniTask.Delay(System.TimeSpan.FromSeconds(3));
            ChartManager.Instance.TestStart2();
            
        }

        private void Update()
        {
            if (!isStart) return;
            timer += Time.deltaTime;
            // 更新当前的节拍数
            curBeat = timer * (bpm / 60f);

            // 遍历每个轨道的音符队列
            for (int pos = 0; pos < _canHitTapNotesQueues.Count; pos++)
            {
                // 将已经进入判定区的音符加入可判定音符队列
                while (curNotesIndexList[pos] < ChartManager.CurChart.notes[pos].Count &&
                       curBeat + noteMissRange * (bpm / 60f) >=
                       ChartManager.CurChart.notes[pos][curNotesIndexList[pos]].beat)
                {
                    Note curChartNote = ChartManager.CurChart.notes[pos][curNotesIndexList[pos]];
                    curChartNote.id = curNotesIndexList[pos];
                    if (curChartNote.auto)
                    {
                        // 处理自动播放音符
                        ResolveAutoNote(curChartNote, pos, curChartNote.beat - curBeat);
                    }
                    else
                    {
                        // 将音符加入可判定队列
                        switch (curChartNote)
                        {
                            case HoldNote holdNote:
                                _canHoldNotesQueues[pos]
                                    .Enqueue((holdNote.beat, holdNote.endBeat, 0));
                                _canHitTapNotesQueues[pos]
                                    .Enqueue(curChartNote);
                                break;
                            case TapNote:
                                _canHitTapNotesQueues[pos]
                                    .Enqueue(curChartNote);
                                break;
                        }
                    }

                    // 更新音符下标
                    curNotesIndexList[pos]++;
                }

                // 检查可打击的音符中是否有Miss，有则移除
                while (_canHitTapNotesQueues[pos].Count > 0 &&
                       _canHitTapNotesQueues[pos].Peek().beat + noteMissRange * (bpm / 60f) <= curBeat)
                {
                    _canHitTapNotesQueues[pos].Dequeue();
                }

                while (_canHoldNotesQueues[pos].Count > 0 &&
                       _canHoldNotesQueues[pos].Peek().endBeat <= curBeat)
                {
                    _canHoldNotesQueues[pos].Dequeue();
                }

                foreach (var item in _canHoldNotesQueues[pos])
                {
                    if (!NotesObjManager.Instance.HitBoxes[pos].isTouching)
                    {
                        Debug.LogWarning("hold断了");
                    }
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
            _canHitTapNotesQueues = new List<Queue<Note>>(KeysCount);
            _canHoldNotesQueues = new List<Queue<(float, float, float)>>(KeysCount);
            for (int i = 0; i < KeysCount; i++)
            {
                curNotesIndexList.Add(0);
                _canHitTapNotesQueues.Add(new Queue<Note>());
                _canHoldNotesQueues.Add(new Queue<(float, float, float)>());
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
            if (_canHitTapNotesQueues?[pos] == null ||
                _canHitTapNotesQueues[pos].Count <= 0) return;
            Note note = _canHitTapNotesQueues[pos].Dequeue();
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