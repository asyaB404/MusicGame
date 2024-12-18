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
        public IReadOnlyList<HitBox> HitBoxes => hitBoxes;
        [SerializeField] private List<int> curNotesGobjIndexList;
        private List<Queue<GameObject>> _notesGobjQueues;
        public float speed;
        [SerializeField] private float preSpawnTime;
        [SerializeField] private float noteSpeed;
        private Dictionary<Collider, int> _keyToPos;
        public IReadOnlyDictionary<Collider, int> KeyToPos => _keyToPos;

        public void StateReset()
        {
            preSpawnTime = 5 / speed;
            noteSpeed = 3.7f * speed / 5;
            curNotesGobjIndexList = new List<int>(GameManager.KeysCount);
            _notesGobjQueues = new List<Queue<GameObject>>(GameManager.KeysCount);
            _keyToPos = new Dictionary<Collider, int>(GameManager.KeysCount);
            for (int i = 0; i < GameManager.KeysCount; i++)
            {
                curNotesGobjIndexList.Add(0);
                _notesGobjQueues.Add(new Queue<GameObject>());
            }

            var j = 0;
            foreach (var collider in GetComponentsInChildren<Collider>(true))
            {
                _keyToPos.Add(collider, j);
                j++;
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
            float adjustedCurBeat = GameManager.CurBeat + preSpawnTime * (GameManager.Bpm / 60f);
            while (currentNoteIndex < notes.Count &&
                   adjustedCurBeat >= notes[currentNoteIndex].beat)
            {
                var curChartNote = notes[currentNoteIndex];
                float offset = (adjustedCurBeat - notes[currentNoteIndex].beat) * (60f / GameManager.Bpm);

                if (curChartNote is not ChartEvent)
                {
                    SpawnNoteObject(pos, curChartNote, currentNoteIndex, offset);
                }

                currentNoteIndex++;
            }

            curNotesGobjIndexList[pos] = currentNoteIndex; // 更新索引
        }

        private void SpawnNoteObject(int pos, Note note, int noteIndex, float offset = 0)
        {
            var startPos = new Vector3(0, 3.7f - offset * noteSpeed);
            GameObject noteObj = null;
            switch (note)
            {
                case TapNote tapNote:
                    noteObj = Instantiate(notePrefabs[0], keysParents[pos]);
                    break;
                case HoldNote holdNote:
                    noteObj = Instantiate(notePrefabs[0], keysParents[pos]);
                    var t = (holdNote.endBeat - holdNote.beat) * (60f / GameManager.Bpm);
                    var holdLine = Instantiate(notePrefabs[1], noteObj.transform);
                    var spriteRenderer = holdLine.GetComponent<SpriteRenderer>();
                    Vector2 size = spriteRenderer.size;
                    spriteRenderer.size = new(noteSpeed * t, size.y);
                    break;
            }
            noteObj.transform.localPosition = startPos;
            noteObj.transform.DOLocalMoveY(-400, noteSpeed / 2).SetSpeedBased();
            noteObj.name = noteIndex.ToString();

            _notesGobjQueues[pos].Enqueue(noteObj);
        }

        private void CleanupNotes(int pos)
        {
            while (_notesGobjQueues[pos].Count > 0 && _notesGobjQueues[pos].Peek().transform.localPosition.y <= 0)
            {
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

        public void PlayEffect(int pos, int color)
        {
            hitBoxes[pos].PlayEffect(color);
        }
    }
}