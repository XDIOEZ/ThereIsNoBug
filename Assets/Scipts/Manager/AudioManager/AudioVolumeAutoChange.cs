using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolumeAutoChange : MonoBehaviour
{
    [Header("音量渐变设置")]
    [Tooltip("音量渐变持续时间（秒）")]
    public float fadeDuration = 5f;
    
    private Coroutine volumeCoroutine;

    public void StrangeOpenVolume()
    {
        // 启动一个协程 使AudioManager的音效和音乐 逐渐增加到最大值
        if (AudioManager.Instance != null)
        {
            // 如果已有正在运行的协程，先停止它
            if (volumeCoroutine != null)
            {
                StopCoroutine(volumeCoroutine);
            }
            
            // 启动新的音量渐变协程
            volumeCoroutine = StartCoroutine(FadeVolumeToMax());
        }
        else
        {
            Debug.LogWarning("AudioManager实例未找到！");
        }
    }
    
    /// <summary>
    /// 音量渐变到最大值的协程
    /// </summary>
    private IEnumerator FadeVolumeToMax()
    {
        if (AudioManager.Instance == null)
            yield break;
            
        float startBGMVolume = AudioManager.Instance.bgmVolume;
        float startSFXVolume = AudioManager.Instance.sfxVolume;
        float targetVolume = 1f;
        float elapsedTime = 0f;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / fadeDuration);
            
            // 渐变BGM音量
            float newBGMVolume = Mathf.Lerp(startBGMVolume, targetVolume, progress);
            AudioManager.Instance.SetBGMVolume(newBGMVolume);
            
            // 渐变SFX音量
            float newSFXVolume = Mathf.Lerp(startSFXVolume, targetVolume, progress);
            AudioManager.Instance.SetSFXVolume(newSFXVolume);
            
            yield return null;
        }
        
        // 确保最终音量为最大值
        AudioManager.Instance.SetBGMVolume(targetVolume);
        AudioManager.Instance.SetSFXVolume(targetVolume);
        
        volumeCoroutine = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}