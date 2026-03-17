using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // 單例模式

    [Header("組件連結")]
    public AudioSource bgmSource; // 用來播背景音樂的喇叭

    [Header("音樂清單")]
    public AudioClip introMusic;    // 一開始的音樂
    public AudioClip tensionMusic;  // (預留) 發現真相時的音樂
    public AudioClip solvedMusic;   // (預留) 解謎後的音樂

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // 遊戲一開始，自動播放開場音樂
        PlayMusic(introMusic);
    }

    // 給其他腳本呼叫的切換功能
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        // 如果現在播的跟想要播的一樣，就不要打斷它
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        bgmSource.clip = clip;
        bgmSource.loop = true; // BGM 一定要循環
        bgmSource.Play();
    }

    // (選用) 淡出淡入效果以後可以加在這裡
}