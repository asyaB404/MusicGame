using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class BpmTest : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private float bpm;

    private void Start()
    {
        DelayedTask().Forget(); // 使用 Forget 来处理异步任务
    }

    private async UniTask DelayedTask()
    {
        await UniTask.Delay(3000);
        InvokeRepeating(nameof(PlaySound), 60 / bpm, 4);
    }

    private void PlaySound()
    {
        source.Play();
    }

    private bool _playing;

    [ContextMenu(nameof(Repeat))]
    private void Repeat()
    {
        _playing = !_playing;
        RepeatTask().Forget();
    }

    private async UniTask RepeatTask()
    {
        while (_playing)
        {
            PlaySound();
            await UniTask.Delay(System.TimeSpan.FromSeconds(60 / bpm));
        }
    }
}