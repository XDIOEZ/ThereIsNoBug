using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tiger : MonoBehaviour
{
    public float eatSheepTime = 1;
    public UnityEvent onPass;
    public UnityEvent onfail;
    public UnityEvent onTigerAwake;
    public UnityEvent onTigerSleep;

    [Header("音频设置")]
    [Tooltip("背景音乐名称")]
    public string bgmName = "AugHHH";
    [Tooltip("第一个音效名称")]
    public string firstSFXName = "VineBoom";
    [Tooltip("第二个音效名称")]
    public string secondSFXName = "GetOut";

    private bool isTigerAwake = false; // 老虎是否处于惊醒状态

    public void Start()
    {
        AudioManager.Instance.OnAudioSettingsChanged.AddListener(CheckIsPass);
    }

    public void CheckIsPass()
    {
        // 如果音量小于等于0.01f，游戏通过
        if (AudioManager.Instance.bgmVolume <= 0.01f)
        {
            Debug.Log("Game Pass");
            onPass?.Invoke();
        }
        else if(AudioManager.Instance.bgmVolume >= 0.01f)
        {
            Debug.Log("Game Fail");
            onfail.Invoke();
        }
    }

    private void Update()
    {
        // 2D射线检测点击老虎
        if (Input.GetMouseButtonDown(0))
        {
            CheckTigerClick();
        }
    }

    /// <summary>
    /// 检测玩家是否点击了老虎
    /// </summary>
    private void CheckTigerClick()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        // 如果射线击中了老虎且满足惊醒条件
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            // 只有当音量大于0.01f且老虎未惊醒时，点击老虎才会惊醒它
            if (AudioManager.Instance.bgmVolume > 0.01f && !isTigerAwake)
            {
                WakeUpTiger();
            }
        }
    }

    /// <summary>
    /// 惊醒老虎
    /// </summary>
    private void WakeUpTiger()
    {
        isTigerAwake = true;
        onTigerAwake?.Invoke();
        AudioManager.Instance.StopBGM(bgmName);
        AudioManager.Instance.PlaySFXWithCallback(firstSFXName, () =>
        {
            AudioManager.Instance.PlaySFXWithCallback(secondSFXName, () =>
            {
                // 启动协程，在eatSheepTime秒后让老虎睡去
                StartCoroutine(PutTigerToSleep());
            });
        });
    }

    /// <summary>
    /// 让老虎睡去的协程
    /// </summary>
    private IEnumerator PutTigerToSleep()
    {
        yield return new WaitForSeconds(eatSheepTime);
        
        isTigerAwake = false;
        onTigerSleep?.Invoke();
        AudioManager.Instance.PlayBGM(bgmName);
    }

    private void OnDisable()
    {
        // 移除事件监听器，防止内存泄漏
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.OnAudioSettingsChanged.RemoveListener(CheckIsPass);
        }
    }
}