using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAudio : MonoBehaviour
{
    [Tooltip("淡入时间")]
    public float FadeInTime = 5;
    [Tooltip("淡出时间")]
    public float FadeOutTime = 5;
    [Tooltip("提前播放多少秒")]
    public float PrePlayTime = 0;
public void PlayBGM(string name)
{
    // 计算实际开始时间：如果PrePlayTime为3，则从第3秒开始播放
    float startTime = PrePlayTime;
    AudioManager.Instance.PlayBGMFromTime(name, startTime);
}

public void PlaySFX(string name)
{
    // 计算实际开始时间：如果PrePlayTime为3，则从第3秒开始播放
    float startTime = PrePlayTime;
    AudioManager.Instance.PlaySFXFromTime(name, startTime);
}
    
    public void SwitchBGM(string name)
    {
        AudioManager.Instance.SwitchBGM(name);
    }
    
    public void StopBGM(string name)
    {
        AudioManager.Instance.StopBGM(name, FadeOutTime);
    }
    
    /// <summary>
    /// 从指定时间开始播放背景音乐
    /// </summary>
    /// <param name="name">音频名称</param>
    /// <param name="startTime">开始时间（秒）</param>
    public void PlayBGMFromTime(string name, float startTime)
    {
        AudioManager.Instance.PlayBGMFromTime(name, startTime);
    }
    
    /// <summary>
    /// 从指定时间开始播放音效
    /// </summary>
    /// <param name="name">音频名称</param>
    /// <param name="startTime">开始时间（秒）</param>
    public void PlaySFXFromTime(string name, float startTime)
    {
        AudioManager.Instance.PlaySFXFromTime(name, startTime);
    }
}