using System.Collections.Generic;
using UnityEngine;

namespace GamePlay
{
    public class NotesObjManager : MonoManager<NotesObjManager>
    {
        [SerializeField] private GameObject notePrefabs;
        [SerializeField] private List<int> curNotesGobjIndexList = new(ChartManager.KeysCount);
    }
}