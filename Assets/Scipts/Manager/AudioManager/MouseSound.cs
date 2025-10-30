using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseSound : MonoBehaviour
{
    public AudioSource audioSource;
    
    [Header("音效控制参数")]
    [Tooltip("播放音效的最小时间间隔（秒）")]
    public float minInterval = 0.5f;
    
    [Tooltip("播放音效的概率（0-1之间）")]
    [Range(0f, 1f)]
    public float playProbability = 0.3f;
    
    private Vector2 lastMousePosition;
    private float lastPlayTime;

    // Start is called before the first frame update
    void Start()
    {
        // 初始化鼠标位置
        lastMousePosition = Input.mousePosition;
        lastPlayTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //检查是不是在老虎的 场景 在老虎的场景下才播放音效
        if(currentPosition.Instance.Y_currentindex == 2 && currentPosition.Instance.X_currentindex == 1)
        {


             if (AudioManager.Instance.bgmVolume <= 0f || SceneManager.GetActiveScene().name != "SceneTest")
             {
                 return;
             }
             audioSource.volume = AudioManager.Instance.bgmVolume;
             // 检查鼠标是否移动
             if (HasMouseMoved())
             {
                 // 检查是否满足播放条件
                 if (CanPlaySound())
                 {
                     PlayMouseSound();
                 }
                 
                 // 更新鼠标位置
                 lastMousePosition = Input.mousePosition;
             }


        }
        else
        {
            Tiger.Instance.ForceTigerToSleep();
        }



    }

    /// <summary>
    /// 检查鼠标是否移动
    /// </summary>
    /// <returns>如果鼠标移动返回true，否则返回false</returns>
    private bool HasMouseMoved()
    {
        Vector2 currentMousePosition = Input.mousePosition;
        return Vector2.Distance(currentMousePosition, lastMousePosition) > 0.1f;
    }
    
    /// <summary>
    /// 检查是否可以播放音效
    /// </summary>
    /// <returns>可以播放返回true，否则返回false</returns>
    private bool CanPlaySound()
    {
        // 检查时间间隔
        if (Time.time - lastPlayTime < minInterval)
        {
            return false;
        }
        
        // 检查播放概率
        if (Random.value > playProbability)
        {
            return false;
        }
        
        return true;
    }
    
    /// <summary>
    /// 播放鼠标移动音效
    /// </summary>
    private void PlayMouseSound()
    {
        if (audioSource != null)
        {
            // 不再检查是否正在播放，直接播放
            audioSource.Play();
            lastPlayTime = Time.time;
            Tiger.Instance.CheckTigerClick(false);
        }
    }
}