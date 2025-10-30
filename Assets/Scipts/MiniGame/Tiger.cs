using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Tiger : MonoBehaviour
{
    #region Variables

    public static Tiger Instance;

    [Header("游戏设置")]
    public float eatSheepTime = 1f;
    public float AudioOffsetValue = 1f;
    public bool CanTakeKey = false;

    [Header("事件")]
    public UnityEvent onPass;
    public UnityEvent onfail;
    public UnityEvent onTigerAwake;
    public UnityEvent onTigerSleep;

    [Header("组件引用")]
    public SkeletonAnimation skeletonAnimation; // 骨骼动画组件
    public GameObject skeletonPrefab;
    
    private bool isTigerAwake = false; // 老虎是否处于惊醒状态

    #endregion

    #region Unity Lifecycle

    void Start()
    {
        Instance = this;
        AudioManager.Instance.OnAudioSettingsChanged.AddListener(CheckIsPass);
        skeletonPrefab.SetActive(false);
    }

void Update()
{
    if(currentPosition.Instance.Y_currentindex!=2)
    {
        skeletonPrefab.SetActive(false);
    }
    else
    {
        skeletonPrefab.SetActive(true);
    }
    
    // 2D射线检测点击老虎
    if (Input.GetMouseButtonDown(0))
    {

        CheckTigerClick(true);
    }
}

/// <summary>
/// 强制将老虎切换为睡眠状态
/// </summary>
public void ForceTigerToSleep()
{
    // 只有当老虎处于惊醒状态时才需要强制切换
    if (isTigerAwake)
    {
        isTigerAwake = false;
        Debug.Log("Tiger forced to sleep");
        onTigerSleep?.Invoke();
        
        // 停止可能正在运行的睡眠协程，避免重复触发
        StopCoroutine(PutTigerToSleep());
    }
}

    void OnDisable()
    {
        // 移除事件监听器，防止内存泄漏
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.OnAudioSettingsChanged.RemoveListener(CheckIsPass);
        }
    }

    #endregion

    #region Game Logic

   /// <summary>
/// 检查游戏是否通过
/// </summary>
public void CheckIsPass()
{
    // 如果音量小于等于0.01f，游戏通过
    if (AudioManager.Instance.bgmVolume <= 0.01f)
    {
        Debug.Log("Game Pass");
        
        onPass?.Invoke();

        CanTakeKey = true;
        
        // 获取Collider组件并使其失去活性
        Collider2D collider = GetComponent<Collider2D>();
            StartCoroutine(PutTigerToSleep());
            if (collider != null)
{
    // 将碰撞体的大小调为0
    collider.enabled = false;
    
    // 或者调整碰撞体大小为0（根据碰撞体类型）
    if (collider is BoxCollider2D boxCollider)
    {
        boxCollider.size = Vector2.zero;
    }
}
    }
   /* else if (AudioManager.Instance.bgmVolume >= 0.01f)
    {
        Debug.Log("Game Fail");
        onfail?.Invoke();
        CanTakeKey = false;
    }*/
}

/// <summary>
/// 检测玩家是否点击了老虎
/// </summary>
public void CheckTigerClick(bool isClick)
{
    if (currentPosition.Instance.Y_currentindex != 2)
        return;

    // 首先检查是否点击在UI上，如果点击在UI上则直接返回
    if (IsPointerOverUI())
        return;

    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

    // 如果射线击中了老虎且满足惊醒条件
    if (hit.collider != null && hit.collider.gameObject == gameObject)
    {
        Debug.Log("click tiger");
            if(isClick)
            AudioManager.Instance.PlaySFX_("狮子吼",5f);
        // 只有当音量大于0.01f且老虎未惊醒时，点击老虎才会惊醒它
        if (AudioManager.Instance.bgmVolume > 0.01f && !isTigerAwake)
        {
            Debug.Log("wake");
            WakeUpTiger();
        }
    }
}

/// <summary>
/// 检测鼠标是否点击在UI上
/// </summary>
/// <returns>如果点击在UI上返回true，否则返回false</returns>
private bool IsPointerOverUI()
{
    // 使用EventSystem检测是否点击在UI上
    if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
    {
        return true;
    }
    
    return false;
}

    /// <summary>
    /// 惊醒老虎
    /// </summary>
    private void WakeUpTiger()
    {
        isTigerAwake = true;
        onTigerAwake?.Invoke();
        StartCoroutine(PutTigerToSleep());
    }

    /// <summary>
    /// 让老虎睡去的协程
    /// </summary>
    private IEnumerator PutTigerToSleep()
    {
        yield return new WaitForSeconds(eatSheepTime);

        isTigerAwake = false;
        Debug.Log("sleep");
        onTigerSleep?.Invoke();
    }

    #endregion

    #region Animation Control

    /// <summary>
    /// 播放循环动画
    /// </summary>
    /// <param name="animationName">动画名称</param>
    public void PalyAnimationLoop(string animationName)
    {
        skeletonAnimation.state.SetAnimation(0, animationName, true);
    }

    /// <summary>
    /// 播放非循环动画
    /// </summary>
    /// <param name="animationName">动画名称</param>
    public void PalyAnimationNoLoop(string animationName)
    {
        skeletonAnimation.state.SetAnimation(0, animationName, false);
    }

    /// <summary>
    /// 等待上一个骨骼动画播放完毕再播放新的动画
    /// </summary>
    /// <param name="animationName">要播放的新动画名称</param>
    [Tooltip("等待上一个骨骼动画播放完毕再播放这个")]
    public void PlayAnimationWaitLastAnimation(string animationName)
    {
        if (skeletonAnimation == null)
            return;

        // 获取当前正在播放的动画
        var currentTrackEntry = skeletonAnimation.state.GetCurrent(0);

        if (currentTrackEntry != null)
        {
            // 如果有正在播放的动画，则等待其完成后再播放新动画
            StartCoroutine(WaitForAnimationComplete(currentTrackEntry, animationName));
        }
        else
        {
            // 如果没有正在播放的动画，直接播放新动画
            skeletonAnimation.state.SetAnimation(0, animationName, true);
        }
    }

    /// <summary>
    /// 等待当前动画播放完毕后播放新动画的协程
    /// </summary>
    /// <param name="trackEntry">当前动画轨道条目</param>
    /// <param name="animationName">要播放的新动画名称</param>
    /// <returns>协程枚举器</returns>
    private IEnumerator WaitForAnimationComplete(TrackEntry trackEntry, string animationName)
    {
        // 等待当前动画播放完毕
        while (trackEntry.IsComplete == false)
        {
            yield return null;
        }

        // 播放新动画，不循环播放
        skeletonAnimation.state.SetAnimation(0, animationName, true);
    }

    #endregion

    #region Audio Control

    /// <summary>
    /// 播放偏移时间的音频
    /// </summary>
    /// <param name="audioName">音频名称</param>
    public void PlayAudioOffsetTime(string audioName)
    {
        AudioManager.Instance.PlaySFXFromTime(audioName, AudioOffsetValue);
    }

    #endregion
}