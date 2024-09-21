using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class BpmTest : MonoBehaviour
{
    [SerializeField] private AudioSource music;

    [FormerlySerializedAs("source")] [SerializeField]
    private AudioSource tick;

    [SerializeField] private float bpm;
    [SerializeField] private int musicOffset;

    private bool _playing;

    [ContextMenu(nameof(Repeat))]
    private void Repeat()
    {
        _playing = !_playing;
        if (_playing)
        {
            music.Play();
            RepeatTask().Forget();
        }
        else
        {
            music.Stop();
        }
    }

    private async UniTask RepeatTask()
    {
        tick.pitch = 1f;
        await UniTask.Delay(musicOffset);
        while (_playing)
        {
            tick.Play();
            tick.pitch += 1 / 3f;
            if (tick.pitch > 2f) tick.pitch = 1;
            await UniTask.Delay(System.TimeSpan.FromSeconds(60 / bpm));
        }
    }
}