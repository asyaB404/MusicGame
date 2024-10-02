using System.Collections.Generic;
using DG.Tweening;
using GamePlay.Notes;
using UnityEngine;

namespace GamePlay
{
    public class NotesObjManager : MonoManager<NotesObjManager>
    {
        [SerializeField] private GameObject[] notePrefabs;
        [SerializeField] private Transform[] keysParents;
        [SerializeField] private List<int> curNotesGobjIndexList = new(GameManager.KeysCount);
        private readonly List<Queue<GameObject>> _notesGobjQueues = new(GameManager.KeysCount);
        public float speed;
        private float _spawnTime;

        public void StateReset()
        {
            _spawnTime = 5 / speed;
            curNotesGobjIndexList.Clear();
            _notesGobjQueues.Clear();
            for (int i = 0; i < GameManager.KeysCount; i++)
            {
                curNotesGobjIndexList.Add(0);
                _notesGobjQueues.Add(new Queue<GameObject>());
            }
        }

        private void Update()
        {
            if (!GameManager.IsStart) return;
            for (int pos = 0; pos < curNotesGobjIndexList.Count; pos++)
            {
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
                        var startPos = new Vector3(0, 4);
                        noteObj = Instantiate(notePrefabs[i],keysParents[pos],false);
                        noteObj.transform.localPosition = startPos;
                        noteObj.transform.DOLocalMoveY(-4, 3.7f / _spawnTime).SetSpeedBased();
                        noteObj.name = curNotesGobjIndexList[pos].ToString();
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

        public void ResolveNote(Note checkNote, int pos = 0)
        {
            if (_notesGobjQueues[pos].Count <= 0) return;
            if (_notesGobjQueues[pos].Peek().name == checkNote.id.ToString())
            {
                GameObject noteObj = _notesGobjQueues[pos].Dequeue();
                Destroy(noteObj);
            }
            else
            {
                Debug.LogWarning("note对应不上");
            }
        }
    }
}