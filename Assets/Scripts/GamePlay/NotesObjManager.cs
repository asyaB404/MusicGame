using System.Collections.Generic;
using GamePlay.Notes;
using UnityEngine;

namespace GamePlay
{
    public class NotesObjManager : MonoManager<NotesObjManager>
    {
        [SerializeField] private GameObject notePrefabs;
        [SerializeField] private List<int> curNotesGobjIndexList = new(GameManager.KeysCount);
        private readonly List<Queue<Note>> _notesGobjQueues = new(GameManager.KeysCount);
        public float speed;
        private float _spawnTime;

        [ContextMenu("重置")]
        public void StateReset()
        {
            _spawnTime = 5 / speed;
            curNotesGobjIndexList.Clear();
            for (int i = 0; i < GameManager.KeysCount; i++)
            {
                curNotesGobjIndexList.Add(0);
            }
        }

        private void Update()
        {
            if (!GameManager.IsStart) return;
            for (int pos = 0; pos < curNotesGobjIndexList.Count; pos++)
            {
                // 将已经进入判定区的音符加入队列
                while (curNotesGobjIndexList[pos] < ChartManager.Chart.notes[pos].Count &&
                       GameManager.CurBeat + _spawnTime >=
                       ChartManager.Chart.notes[pos][curNotesGobjIndexList[pos]].beat)
                {
                    Note curChartNote = ChartManager.Chart.notes[pos][curNotesGobjIndexList[pos]];
                    if (curChartNote.auto)
                    {
                    }
                    else
                    {
                        _notesGobjQueues[pos].Enqueue(ChartManager.Chart.notes[pos][curNotesGobjIndexList[pos]]);
                    }

                    curNotesGobjIndexList[pos]++;
                }

                while (_notesGobjQueues[pos].Count > 0 &&
                       _notesGobjQueues[pos].Peek().beat + 0.3 / speed * (GameManager.Bpm / 60) <=
                       GameManager.CurBeat)
                {
                    Debug.Log("miss");
                    _notesGobjQueues[pos].Dequeue();
                }
            }
        }
    }
}