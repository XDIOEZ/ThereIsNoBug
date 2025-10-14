using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通用音频管理器（AudioManager）
/// 用于统一管理游戏中的背景音乐（BGM）与音效（SFX）
/// </summary>
public class AudioManager : MonoBehaviour
{
    #region 🔷 单例模式与基础变量

    public static AudioManager Instance; // 全局访问入口

    [Header("Audio Sources")]
    public AudioSource bgmSource;      // 背景音乐播放器
    public AudioSource sfxSource;      // 音效播放器（用于播放一次性音效）

    [Header("Audio Clips")]
    public List<AudioClip> bgmClips;   // 背景音乐资源列表
    public List<AudioClip> sfxClips;   // 音效资源列表

    // 用于快速通过名字查找音频
    private Dictionary<string, AudioClip> bgmDict = new();
    private Dictionary<string, AudioClip> sfxDict = new();

    [Header("音量设置")]
    [Range(0, 1)] public float bgmVolume = 1f; // 背景音乐音量
    [Range(0, 1)] public float sfxVolume = 1f; // 音效音量
    public bool isMuted = false;               // 是否静音

    #endregion

    #region 🧩 生命周期

    private void Awake()
    {
        // 确保全局唯一的单例实例
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // 切换场景时不销毁

        // 构建BGM与SFX的字典，方便后续通过名称播放
        foreach (var clip in bgmClips)
        {
            if (clip != null && !bgmDict.ContainsKey(clip.name))
                bgmDict.Add(clip.name, clip);
        }
        foreach (var clip in sfxClips)
        {
            if (clip != null && !sfxDict.ContainsKey(clip.name))
                sfxDict.Add(clip.name, clip);
        }
    }

    #endregion

    #region 🎵 背景音乐控制

    /// <summary>
    /// 播放背景音乐（支持淡入）
    /// </summary>
    /// <param name="name">音乐名（AudioClip.name）</param>
    /// <param name="loop">是否循环播放</param>
    /// <param name="fadeTime">淡入时长（秒）</param>
    public void PlayBGM(string name, bool loop = true, float fadeTime = 0.5f)
    {
        if (!bgmDict.TryGetValue(name, out var clip))
        {
            Debug.LogWarning($"[AudioManager] 未找到BGM: {name}");
            return;
        }

        StopAllCoroutines(); // 停止之前的淡出协程
        StartCoroutine(FadeInBGM(clip, loop, fadeTime));
    }

    /// <summary>
    /// 淡入播放BGM（协程）
    /// </summary>
    private IEnumerator FadeInBGM(AudioClip clip, bool loop, float fadeTime)
    {
        // 若已有音乐在播放，先淡出
        if (bgmSource.isPlaying)
            yield return StartCoroutine(FadeOutBGM(fadeTime));

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.volume = 0f;
        bgmSource.Play();

        float t = 0;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            bgmSource.volume = Mathf.Lerp(0, bgmVolume, t / fadeTime);
            yield return null;
        }
        bgmSource.volume = bgmVolume;
    }

    /// <summary>
    /// 淡出停止BGM（协程）
    /// </summary>
    private IEnumerator FadeOutBGM(float fadeTime)
    {
        float startVol = bgmSource.volume;
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            bgmSource.volume = Mathf.Lerp(startVol, 0, t / fadeTime);
            yield return null;
        }
        bgmSource.Stop();
    }

    /// <summary>
    /// 手动停止BGM（带淡出效果）
    /// </summary>
    public void StopBGM(float fadeTime = 0.5f)
    {
        StartCoroutine(FadeOutBGM(fadeTime));
    }

    #endregion

    #region 🔊 音效控制（SFX）

    /// <summary>
    /// 播放音效（PlayOneShot方式，不会打断BGM）
    /// </summary>
    /// <param name="name">音效名称（AudioClip.name）</param>
    public void PlaySFX(string name)
    {
        if (!sfxDict.TryGetValue(name, out var clip))
        {
            Debug.LogWarning($"[AudioManager] 未找到SFX: {name}");
            return;
        }
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    #endregion

    #region ⚙️ 音量与静音控制

    /// <summary>
    /// 设置背景音乐音量
    /// </summary>
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        if (bgmSource != null) bgmSource.volume = bgmVolume;
    }

    /// <summary>
    /// 设置音效音量
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    /// <summary>
    /// 设置静音开关
    /// </summary>
    public void SetMute(bool mute)
    {
        isMuted = mute;
        bgmSource.mute = mute;
        sfxSource.mute = mute;
    }

    #endregion
}
