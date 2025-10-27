using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAudio : MonoBehaviour
{
    [Tooltip("淡入时间")]
    public float FadeInTime = 5;
    [Tooltip("淡出时间")]
    public float FadeOutTime = 5;
    public void PlayBGM(string name)
    {
        AudioManager.Instance.PlayBGM(name,true, FadeInTime);
    }
    
    public void PlaySFX(string name)
    {
        AudioManager.Instance.PlaySFX(name);
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