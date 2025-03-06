using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("-AudioSources-")]
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource seSourcePrefab;

    [Header("-���ʐݒ�-")]
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
    /// ���ʐݒ�����[�h
    /// </summary>
    private void LoadVolumeSettings()
    {
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        seVolume = PlayerPrefs.GetFloat("SEVolume", 1f);
        bgmSource.volume = bgmVolume;
    }

    /// <summary>
    /// BGM�̍Đ�
    /// </summary>
    /// <param name="clip">��������BGM</param>
    /// <param name="fadeTime">�t�F�[�h�C�����Ă���b��</param>
    public void PlayBGM(AudioClip clip, float fadeTime = 1f)
    {
        if (bgmSource.clip == clip) return;
        StartCoroutine(FadeBGM(clip, fadeTime));
    }

    /// <summary>
    /// BGM�̃t�F�[�h�C��
    /// </summary>
    /// <param name="newClip">����BGM</param>
    /// <param name="fadeTime">�t�F�[�h�C�����Ă���b��</param>
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
    /// BGM�̒�~
    /// </summary>
    /// <param name="fadeTime">�t�F�[�h�A�E�g����b��</param>
    public void StopBGM(float fadeTime = 1f)
    {
        StartCoroutine(FadeOutBGM(fadeTime));
    }

    /// <summary>
    /// BGM�̃t�F�[�h�A�E�g
    /// </summary>
    /// <param name="fadeTime">�t�F�[�h�A�E�g����b��</param>
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
    /// SE�̍Đ�
    /// </summary>
    /// <param name="clip">��������SE</param>
    public void PlaySE(AudioClip clip)
    {
        AudioSource seSource = Instantiate(seSourcePrefab, transform);
        seSource.clip = clip;
        seSource.volume = seVolume;
        seSource.Play();
        Destroy(seSource.gameObject, clip.length);
    }

    /// <summary>
    /// BGM�̃{�����[�����Z�b�g
    /// </summary>
    /// <param name="volume">�{�����[��</param>
    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    /// <summary>
    /// SE�̃{�����[�����Z�b�g
    /// </summary>
    /// <param name="volume">�{�����[��</param>
    public void SetSEVolume(float volume)
    {
        seVolume = volume;
        PlayerPrefs.SetFloat("SEVolume", volume);
    }
}
