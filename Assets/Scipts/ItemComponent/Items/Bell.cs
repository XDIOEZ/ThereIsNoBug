using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;

public class Bell : Item
{
    private InteractableComponent interactableComponent;
    public UnityEvent OnInteract;
    
    public SkeletonAnimation skeletonAnimation; // 骨骼动画组件
    public GameObject skeletonPrefab;

    private void Start()
    {
        interactableComponent = GetComponent<InteractableComponent>();
        interactableComponent.OnInteract += Used;
    }

    public void Update()
    {
        if(currentPosition.Instance.Y_currentindex!=3)
        {
            skeletonPrefab.SetActive(false);
        }
        else
        {
            skeletonPrefab.SetActive(true);
        }
    }


    protected override void Used()
    {
        base.Used();
        OnInteract?.Invoke();
        Click();
    }

    private void Click()
    {
        Debug.Log("BellClick");
        skeletonAnimation.state.SetAnimation(0, "posun1", false);
        PlayAnimationWaitLastAnimation("posun2");
        //StartCoroutine(CountTime());
    }

    
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
    
    protected override IEnumerator CountTime()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
