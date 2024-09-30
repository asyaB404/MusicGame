using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GamePlay.Notes;
using UnityEngine;

namespace GamePlay
{
    public class NotesObjManager : MonoManager<NotesObjManager>
    {
        [SerializeField] private GameObject[] notePrefabs;
        [SerializeField] private List<int> curNotesGobjIndexList = new(GameManager.KeysCount);
        private readonly List<Queue<GameObject>> _notesGobjQueues = new(GameManager.KeysCount);
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
                    var curChartNote = ChartManager.Chart.notes[pos][curNotesGobjIndexList[pos]];
                    if (curChartNote.auto)
                    {
                    }
                    else
                    {
                        var note = ChartManager.Chart.notes[pos][curNotesGobjIndexList[pos]];
                        GameObject noteObj;
                        int i = note switch
                        {
                            TapNote => 0,
                            _ => 0
                        };
                        noteObj = Instantiate(notePrefabs[i]);
                        _notesGobjQueues[pos].Enqueue(noteObj);
                    }

                    curNotesGobjIndexList[pos]++;
                }

                while (_notesGobjQueues[pos].Count > 0 &&
                       _notesGobjQueues[pos].Peek().transform.localPosition.y <= 0)
                {
                    Debug.Log("destroy");
                    _notesGobjQueues[pos].Dequeue();
                }
            }
        }
        
        public void ResolveNote(int pos = 0)
        {
            if (_notesGobjQueues[pos].Count <= 0) return;
            GameObject noteObj = _notesGobjQueues[pos].Dequeue();
            Destroy(noteObj);
        }
    }
}