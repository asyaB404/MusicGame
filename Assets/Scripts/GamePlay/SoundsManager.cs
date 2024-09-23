using System.Collections.Generic;
using UnityEngine;

namespace GamePlay
{
    public enum SoundType
    {
        Tick,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Tap,
    }

    public class SoundsManager : MonoBehaviour
    {
        public static SoundsManager Instance { get; private set; }
        public List<AudioClip> sounds = new();

        private void Awake()
        {
            Instance = this;
        }
    }
}