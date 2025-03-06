using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("-AudioSources-")]
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource seSourcePrefab;

    [Header("-音量設定-")]
    [Range(0f, 1f)] public float bgmVolume = 1f;
    [Range(0f, 1f)] public float seVolume = 1f;

    Dictionary<string, AudioClip> soundEffects = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadVolumeSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 音量設定をロード
    /// </summary>
    private void LoadVolumeSettings()
    {
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        seVolume = PlayerPrefs.GetFloat("SEVolume", 1f);
        bgmSource.volume = bgmVolume;
    }

    /// <summary>
    /// BGMの再生
    /// </summary>
    /// <param name="clip">流したいBGM</param>
    /// <param name="fadeTime">フェードインしてくる秒数</param>
    public void PlayBGM(AudioClip clip, float fadeTime = 1f)
    {
        if (bgmSource.clip == clip) return;
        StartCoroutine(FadeBGM(clip, fadeTime));
    }

    /// <summary>
    /// BGMのフェードイン
    /// </summary>
    /// <param name="newClip">流すBGM</param>
    /// <param name="fadeTime">フェードインしてくる秒数</param>
    private IEnumerator FadeBGM(AudioClip newClip, float fadeTime)
    {
        float startVolume = bgmSource.volume;
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
            yield return null;
        }

        bgmSource.clip = newClip;
        bgmSource.Play();

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(0, bgmVolume, t / fadeTime);
            yield return null;
        }
        bgmSource.volume = bgmVolume;
    }

    /// <summary>
    /// BGMの停止
    /// </summary>
    /// <param name="fadeTime">フェードアウトする秒数</param>
    public void StopBGM(float fadeTime = 1f)
    {
        StartCoroutine(FadeOutBGM(fadeTime));
    }

    /// <summary>
    /// BGMのフェードアウト
    /// </summary>
    /// <param name="fadeTime">フェードアウトする秒数</param>
    /// <returns></returns>
    private IEnumerator FadeOutBGM(float fadeTime)
    {
        float startVolume = bgmSource.volume;
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
            yield return null;
        }
        bgmSource.Stop();
        bgmSource.clip = null;
    }

    /// <summary>
    /// SEの再生
    /// </summary>
    /// <param name="clip">流したいSE</param>
    public void PlaySE(AudioClip clip)
    {
        AudioSource seSource = Instantiate(seSourcePrefab, transform);
        seSource.clip = clip;
        seSource.volume = seVolume;
        seSource.Play();
        Destroy(seSource.gameObject, clip.length);
    }

    /// <summary>
    /// BGMのボリュームをセット
    /// </summary>
    /// <param name="volume">ボリューム</param>
    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    /// <summary>
    /// SEのボリュームをセット
    /// </summary>
    /// <param name="volume">ボリューム</param>
    public void SetSEVolume(float volume)
    {
        seVolume = volume;
        PlayerPrefs.SetFloat("SEVolume", volume);
    }
}
