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
        [SerializeField] private HitBox[] hitBoxes;
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
                var notes = ChartManager.CurChart.notes[pos];
                var currentNoteIndex = curNotesGobjIndexList[pos];

                // 生成音符的逻辑
                SpawnNotes(pos, notes, ref currentNoteIndex);

                // 清理不需要的音符
                CleanupNotes(pos);
            }
        }

        private void SpawnNotes(int pos, List<Note> notes, ref int currentNoteIndex)
        {
            while (currentNoteIndex < notes.Count &&
                   GameManager.CurBeat + _spawnTime >= notes[currentNoteIndex].beat)
            {
                var curChartNote = notes[currentNoteIndex];

                if (curChartNote is not ChartEvent)
                {
                    SpawnNoteObject(pos, curChartNote, currentNoteIndex);
                }

                currentNoteIndex++;
            }

            curNotesGobjIndexList[pos] = currentNoteIndex; // 更新索引
        }

        private void SpawnNoteObject(int pos, Note note, int noteIndex)
        {
            // 使用 switch 来根据不同音符类型获取预制体索引
            var i = note switch
            {
                TapNote => 0, // TapNote 类型
                // SoundNote => 1, // SoundNote 类型
                _ => 0 // ???
            };

            var startPos = new Vector3(0, 4);

            // 生成音符对象
            GameObject noteObj = Instantiate(notePrefabs[i], keysParents[pos], false);
            noteObj.transform.localPosition = startPos;

            // 音符移动动画
            noteObj.transform.DOLocalMoveY(-4, 3.7f / _spawnTime).SetSpeedBased();
            noteObj.name = noteIndex.ToString();

            _notesGobjQueues[pos].Enqueue(noteObj);
        }

        private void CleanupNotes(int pos)
        {
            while (_notesGobjQueues[pos].Count > 0 && _notesGobjQueues[pos].Peek().transform.localPosition.y <= 0)
            {
                Debug.Log("destroy");
                Destroy(_notesGobjQueues[pos].Dequeue());
            }
        }


        public void ResolveNote(Note checkNote, int pos = 0)
        {
            if (_notesGobjQueues[pos].Count <= 0) return;
            if (_notesGobjQueues[pos].Peek().name != checkNote.id.ToString()) return;
            var noteObj = _notesGobjQueues[pos].Dequeue();
            Destroy(noteObj);
        }

        public void PlayEffect(int pos,int color)
        {
            hitBoxes[pos].PlayEffect(color);
        }
    }
}