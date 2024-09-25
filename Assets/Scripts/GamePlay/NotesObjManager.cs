using System.Collections.Generic;
using UnityEngine;

namespace GamePlay
{
    public class NotesObjManager : MonoBehaviour
    {
        [SerializeField] private List<int> curNotesGobjIndexList = new(ChartManager.KeysCount);
    }
}