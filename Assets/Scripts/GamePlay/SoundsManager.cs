using System.Collections.Generic;
using UnityEngine;

namespace GamePlay
{
    public enum SoundType
    {
        Tick,
        Tap
    }

    public class SoundsManager : MonoBehaviour
    {
        public static SoundsManager Instance { get; private set; }
        public IReadOnlyList<AudioClip> Sounds => sounds;
        [SerializeField] private List<AudioClip> sounds = new();


        private void Awake()
        {
            Instance = this;
        }
    }
}