using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class BpmTest : MonoBehaviour
{
    [SerializeField] private AudioSource music;

    [FormerlySerializedAs("source")] [SerializeField]
    private AudioSource tick;

    [SerializeField] private float bpm;

    private bool _playing;

    [ContextMenu(nameof(Repeat))]
    private void Repeat()
    {
        _playing = !_playing;
        if (!_playing) return;
        music.Play();
        RepeatTask().Forget();
    }

    private async UniTask RepeatTask()
    {
        tick.pitch = 1f;
        while (_playing)
        {
            tick.Play();
            tick.pitch += 1 / 3f;
            if (tick.pitch > 2f) tick.pitch = 1;
            await UniTask.Delay(System.TimeSpan.FromSeconds(60 / bpm));
        }
    }
}