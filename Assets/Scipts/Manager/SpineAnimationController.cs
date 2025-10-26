using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

/// <summary>
/// Spine动画控制器，用于统一管理Spine骨骼动画的播放
/// </summary>
public class SpineAnimationController : MonoBehaviour
{
    [Header("组件引用")]
    public SkeletonAnimation skeletonAnimation;

    private void Awake()
    {
        // 如果没有在Inspector中分配，则尝试自动获取组件
        if (skeletonAnimation == null)
        {
            skeletonAnimation = GetComponent<SkeletonAnimation>();
        }
    }

    /// <summary>
    /// 播放循环动画
    /// </summary>
    /// <param name="animationName">动画名称</param>
    public void PlayAnimationLoop(string animationName)
    {
        if (skeletonAnimation == null)
        {
            Debug.LogWarning("SkeletonAnimation组件未分配", this);
            return;
        }

        skeletonAnimation.state.SetAnimation(0, animationName, true);
    }

    /// <summary>
    /// 播放非循环动画
    /// </summary>
    /// <param name="animationName">动画名称</param>
    public void PlayAnimationNoLoop(string animationName)
    {
        if (skeletonAnimation == null)
        {
            Debug.LogWarning("SkeletonAnimation组件未分配", this);
            return;
        }

        skeletonAnimation.state.SetAnimation(0, animationName, false);
    }

    /// <summary>
    /// 等待上一个骨骼动画播放完毕再播放新的动画
    /// </summary>
    /// <param name="animationName">要播放的新动画名称</param>
    public void PlayAnimationWaitLastAnimation(string animationName)
    {
        if (skeletonAnimation == null)
        {
            Debug.LogWarning("SkeletonAnimation组件未分配", this);
            return;
        }

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
            skeletonAnimation.state.SetAnimation(0, animationName, false);
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
        skeletonAnimation.state.SetAnimation(0, animationName, false);
    }

    /// <summary>
    /// 检查是否有动画正在播放
    /// </summary>
    /// <returns>如果有动画正在播放返回true，否则返回false</returns>
    public bool IsAnimationPlaying()
    {
        if (skeletonAnimation == null)
            return false;

        var currentTrackEntry = skeletonAnimation.state.GetCurrent(0);
        return currentTrackEntry != null && !currentTrackEntry.IsComplete;
    }

    /// <summary>
    /// 获取当前播放的动画名称
    /// </summary>
    /// <returns>当前动画名称，如果没有动画播放则返回空字符串</returns>
    public string GetCurrentAnimationName()
    {
        if (skeletonAnimation == null)
            return string.Empty;

        var currentTrackEntry = skeletonAnimation.state.GetCurrent(0);
        return currentTrackEntry?.Animation?.Name ?? string.Empty;
    }

    /// <summary>
    /// 停止当前播放的动画
    /// </summary>
    public void StopCurrentAnimation()
    {
        if (skeletonAnimation == null)
            return;

        skeletonAnimation.state.ClearTracks();
    }
}