using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [Header("相关事件")]
    public UnityEvent OnAudioSettingsChanged;
    public UnityEvent<string> OnPlaySFX;
    public UnityEvent<string> OnPlayBGM;

    [Header("音量设置")]
    [Range(0, 1)] public float bgmVolume = 1f; // 背景音乐音量
    [Range(0, 1)] public float sfxVolume = 1f; // 音效音量
    public bool isMuted = false;               // 是否静音

    // 保存上一帧的音量和静音状态，用于检测变化
    private float previousBGMVolume = 1f;
    private float previousSFXVolume = 1f;
    private bool previousIsMuted = false;

    [Header("3D音效设置")]
    public GameObject audioSourcePrefab; // 用于创建3D音效的AudioSource预制体

    // 用于管理多个BGM播放
    private List<AudioSource> bgmSources = new List<AudioSource>();

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

        // 初始化上一帧的值
        previousBGMVolume = bgmVolume;
        previousSFXVolume = sfxVolume;
        previousIsMuted = isMuted;

        // 初始化主BGM源
        if (bgmSource != null)
        {
            bgmSources.Add(bgmSource);
        }
    }

    private void Update()
    {
        /*// 检测音量或静音状态是否发生变化
        CheckAudioSettingsChanges();*/
    }

    /// <summary>
    /// 检测音频设置变化并及时应用
    /// </summary>
    private void CheckAudioSettingsChanges()
    {
        bool settingsChanged = false;

        // 检查BGM音量是否变化
        if (previousBGMVolume != bgmVolume)
        {
            SetBGMVolume(bgmVolume);
            previousBGMVolume = bgmVolume;
            settingsChanged = true;
        }

        // 检查SFX音量是否变化
        if (previousSFXVolume != sfxVolume)
        {
            SetSFXVolume(sfxVolume);
            previousSFXVolume = sfxVolume;
            settingsChanged = true;
        }

        // 检查静音状态是否变化
        if (previousIsMuted != isMuted)
        {
            SetMute(isMuted);
            previousIsMuted = isMuted;
            settingsChanged = true;
        }

        // 如果有任何设置变化，触发事件并输出日志
        if (settingsChanged)
        {
            OnAudioSettingsChanged?.Invoke();
            Debug.Log($"[AudioManager] 音频设置已更新 - BGM音量: {bgmVolume}, SFX音量: {sfxVolume}, 静音: {isMuted}");
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
    [Button("播放背景音乐")]
    public AudioSource PlayBGM(string name, bool loop = true, float fadeTime = 2f)
    {
        if (!bgmDict.TryGetValue(name, out var clip))
        {
            Debug.LogWarning($"[AudioManager] 未找到BGM: {name}");
            return null;
        }

        // 创建新的AudioSource用于播放BGM
        GameObject bgmObject = new GameObject($"BGM_{name}");
        bgmObject.transform.SetParent(transform);
        AudioSource newBGMSource = bgmObject.AddComponent<AudioSource>();
        newBGMSource.outputAudioMixerGroup = bgmSource != null ? bgmSource.outputAudioMixerGroup : null;
        
        // 配置新的AudioSource
        newBGMSource.clip = clip;
        newBGMSource.loop = loop;
        newBGMSource.volume = 0f;
        newBGMSource.Play();
        
        // 添加到BGM源列表
        bgmSources.Add(newBGMSource);
       

        // 淡入播放
        StartCoroutine(FadeInBGM(newBGMSource, fadeTime));
        
        // 触发播放BGM事件
        OnPlayBGM?.Invoke(name);
        return newBGMSource;
    }

    /// <summary>
    /// 切换背景音乐（淡出当前音乐并淡入新音乐）
    /// </summary>
    /// <param name="name">要切换到的音乐名</param>
    /// <param name="fadeTime">淡入淡出时长（秒）</param>
    /// <param name="loop">是否循环播放</param>
    [Button("切换背景音乐")]
    public void SwitchBGM(string name, float fadeTime = 0.5f, bool loop = true)
    {
        if (!bgmDict.TryGetValue(name, out var clip))
        {
            Debug.LogWarning($"[AudioManager] 未找到BGM: {name}");
            return;
        }

        // 停止当前所有BGM并淡入新BGM
        StartCoroutine(SwitchBGMCoroutine(clip, fadeTime, loop, name));
    }

    /// <summary>
    /// 切换BGM的协程
    /// </summary>
    private IEnumerator SwitchBGMCoroutine(AudioClip newClip, float fadeTime, bool loop, string clipName)
    {
        // 淡出当前所有BGM
        List<Coroutine> fadeOutCoroutines = new List<Coroutine>();
        List<AudioSource> sourcesToStop = new List<AudioSource>(bgmSources);
        
        foreach (var source in sourcesToStop)
        {
            if (source != null && source.isPlaying)
            {
                fadeOutCoroutines.Add(StartCoroutine(FadeOutBGM(source, fadeTime)));
            }
        }

        // 等待所有淡出完成
        foreach (var coroutine in fadeOutCoroutines)
        {
            yield return coroutine;
        }

        // 播放新的BGM
        PlayBGM(clipName, loop, fadeTime);
    }

    /// <summary>
    /// 淡入播放BGM（协程）
    /// </summary>
    private IEnumerator FadeInBGM(AudioSource source, float fadeTime)
    {
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(0, bgmVolume, t / fadeTime);
            yield return null;
        }
        source.volume = bgmVolume;
    }

    /// <summary>
    /// 淡出停止指定的BGM（协程）
    /// </summary>
    private IEnumerator FadeOutBGM(AudioSource source, float fadeTime)
    {
        float startVol = source.volume;
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(startVol, 0, t / fadeTime);
            yield return null;
        }
        source.Stop();
        // 销毁对象并从列表中移除
        if (bgmSources.Contains(source))
        {
            bgmSources.Remove(source);
        }
        Destroy(source.gameObject);
    }

    /// <summary>
    /// 停止指定名称的BGM
    /// </summary>
    /// <param name="name">BGM名称</param>
    /// <param name="fadeTime">淡出时间</param>
    public void StopBGM(string name, float fadeTime = 0.5f)
    {
        AudioSource targetSource = null;
        foreach (var source in bgmSources)
        {
            if (source != null && source.clip != null && source.clip.name == name)
            {
                targetSource = source;
                break;
            }
        }

        if (targetSource != null)
        {
            StartCoroutine(FadeOutBGM(targetSource, fadeTime));
        }
    }

    /// <summary>
    /// 停止所有BGM
    /// </summary>
    /// <param name="fadeTime">淡出时间</param>
    public void StopAllBGM(float fadeTime = 0.5f)
    {
        // 创建副本以避免在迭代时修改列表
        List<AudioSource> sourcesToStop = new List<AudioSource>(bgmSources);
        foreach (var source in sourcesToStop)
        {
            if (source != null)
            {
                StartCoroutine(FadeOutBGM(source, fadeTime));
            }
        }
    }

    /// <summary>
    /// 手动停止主BGM（带淡出效果）
    /// </summary>
    public void StopMainBGM(float fadeTime = 0.5f)
    {
        if (bgmSource != null && bgmSource.isPlaying)
        {
            StartCoroutine(FadeOutBGM(bgmSource, fadeTime));
        }
    }

    #endregion

    #region 🔊 音效控制（SFX）

    /// <summary>
    /// 播放音效（PlayOneShot方式，不会打断BGM）
    /// </summary>
    /// <param name="name">音效名称（AudioClip.name）</param>
    [Button("播放音效")]
    public void PlaySFX(string name)
    {
        if (!sfxDict.TryGetValue(name, out var clip))
        {
            Debug.LogWarning($"[AudioManager] 未找到SFX: {name}");
            return;
        }
        sfxSource.PlayOneShot(clip, sfxVolume);
        
        // 触发播放SFX事件
        OnPlaySFX?.Invoke(name);
    }

    /// <summary>
    /// 播放音效并带回调函数（在音效播放完毕后激活回调函数）
    /// </summary>
    /// <param name="name">音效名称</param>
    /// <param name="callback">音效播放完毕后的回调函数</param>
    [Button("播放音效带回调")]
    public void PlaySFXWithCallback(string name, UnityAction callback)
    {
        if (!sfxDict.TryGetValue(name, out var clip))
        {
            Debug.LogWarning($"[AudioManager] 未找到SFX: {name}");
            return;
        }

        // 创建临时AudioSource来播放音效
        GameObject sfxObject = new GameObject($"SFX_{name}");
        sfxObject.transform.SetParent(transform);
        AudioSource tempSource = sfxObject.AddComponent<AudioSource>();
        
        // 配置AudioSource
        tempSource.clip = clip;
        tempSource.volume = sfxVolume;
        tempSource.outputAudioMixerGroup = sfxSource != null ? sfxSource.outputAudioMixerGroup : null;
        tempSource.Play();
        
        // 触发播放SFX事件
        OnPlaySFX?.Invoke(name);
        
        // 启动协程在音效播放完毕后执行回调并清理
        StartCoroutine(SFXCallbackCoroutine(tempSource, clip.length, callback, sfxObject));
    }

    /// <summary>
    /// SFX回调协程
    /// </summary>
    private IEnumerator SFXCallbackCoroutine(AudioSource source, float clipLength, UnityAction callback, GameObject sfxObject)
    {
        // 等待音效播放完毕
        yield return new WaitForSeconds(clipLength);
        
        // 执行回调函数
        callback?.Invoke();
        
        // 清理临时对象
        Destroy(sfxObject);
    }

    /// <summary>
    /// 在指定位置播放3D音效
    /// </summary>
    /// <param name="name">音效名称</param>
    /// <param name="position">播放位置</param>
    [Button("在特定位置播放音效")]
    public void PlaySFXAtPosition(string name, Vector3 position)
    {
        if (!sfxDict.TryGetValue(name, out var clip))
        {
            Debug.LogWarning($"[AudioManager] 未找到SFX: {name}");
            return;
        }

        if (audioSourcePrefab != null)
        {
            // 创建AudioSource预制体实例
            GameObject audioObject = Instantiate(audioSourcePrefab, position, Quaternion.identity);
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();
            
            if (audioSource != null)
            {
                // 配置AudioSource
                audioSource.clip = clip;
                audioSource.volume = sfxVolume;
                audioSource.spatialBlend = 1.0f; // 设置为3D音效
                audioSource.Play();
                
                // 音效播放完毕后销毁对象
                Destroy(audioObject, clip.length);
            }
            else
            {
                Debug.LogWarning($"[AudioManager] AudioSourcePrefab缺少AudioSource组件");
                Destroy(audioObject);
            }
        }
        else
        {
            // 如果没有设置预制体，则使用默认方式播放
            Debug.LogWarning($"[AudioManager] 未设置AudioSourcePrefab，使用默认方式播放音效");
            PlaySFX(name);
        }
        
        // 触发播放SFX事件
        OnPlaySFX?.Invoke(name);
    }

    #endregion

    #region ⚙️ 音量与静音控制

    /// <summary>
    /// 设置背景音乐音量
    /// </summary>
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        // 更新所有BGM源的音量
        foreach (var source in bgmSources)
        {
            if (source != null)
            {
                source.volume = bgmVolume;
            }
        }
        OnAudioSettingsChanged?.Invoke();
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
        // 更新所有BGM源的静音状态
        foreach (var source in bgmSources)
        {
            if (source != null)
            {
                source.mute = mute;
            }
        }
        bgmSource.mute = mute;
        sfxSource.mute = mute;
    }

    #endregion

    #region UI控制
    [Button("打开UI")]
    public void OpenUI()
    {
        UIMgr.Instance().ShowPanel<BasePanel>("SettingPanel", E_UI_Layer.System, _ =>
        {
            SettingPanel setting = _ as SettingPanel;
            setting.MusicSlider.value = bgmVolume;
            setting.MusicSlider.maxValue = 1;
            setting.MusicSlider.onValueChanged.AddListener(SetBGMVolume);
        });
    }

[Button("关闭UI")]
public void CloseUI()
{
    UIMgr.Instance().HidePanel("SettingPanel");
    // 获取面板实例
    var panel = UIMgr.Instance().GetPanel<BasePanel>("SettingPanel");
    if (panel != null)
    {
        // 转换为SettingPanel类型
        SettingPanel setting = panel as SettingPanel;
        // 移除监听事件
        if (setting != null && setting.MusicSlider != null)
        {
            setting.MusicSlider.onValueChanged.RemoveListener(SetBGMVolume);
        }
    }
}

    #endregion
}