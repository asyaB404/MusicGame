using System.Collections.Generic;
using UnityEngine;

namespace GamePlay
{
    public class SoundsManager : MonoManager<SoundsManager>
    {
        public IReadOnlyList<AudioClip> Sounds => sounds;
        [SerializeField] private List<AudioClip> sounds = new();
    }
}