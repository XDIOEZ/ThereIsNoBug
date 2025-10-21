using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UIhover : MonoBehaviour
{
    private Vector3 originalScale;
    private Tween currentTween;// 当前的动画

    [Tooltip("悬停时目标缩放")]
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1f);

    [Tooltip("动画时长（秒）")]
    public float duration = 0.2f;

    [Tooltip("缓动类型")]
    public Ease ease = Ease.OutBack;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    public void StartHover()
    {
        currentTween?.Kill(); // 停止当前动画
        currentTween = 
            transform.DOScale(hoverScale, duration).SetEase(ease);
    }

    public void ExitHover()
    {
        currentTween?.Kill(); // 停止当前动画
        currentTween =  
            transform.DOScale(originalScale, duration).SetEase(ease);
    }
}
