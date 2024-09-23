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

        private void Awake()
        {
            Instance = this;
        }
    }
}