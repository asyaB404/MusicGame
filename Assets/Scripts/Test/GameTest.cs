using UnityEngine;

namespace Test
{
    public class GameTest : MonoBehaviour
    {
        [SerializeField] private bool isStart;
        [SerializeField] private float bpm;
        [SerializeField] private float curBeat;
        [SerializeField] private float timer;

        private void Update()
        {
            if (!isStart) return;
            timer += Time.deltaTime;
            curBeat = timer * (bpm / 60);
        }

        private void ReStart()
        {
            timer = 0;
            curBeat = 0;
        }
    }
}