using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Sheep : Item
{
    private InteractableComponent InteractableComponent;

    [Header("晃动设置")]
    public float shakeDuration = 0.5f;
    public float shakeStrength = 0.5f;
    public int shakeVibrato = 10;
    public float shakeRandomness = 90f;
    private Vector3 originalPosition;
    private bool isShaking = false;
    private bool isOut;

    private void Start()
    {
        InteractableComponent = GetComponent<InteractableComponent>();
        InteractableComponent.OnInteract += Used;
        originalPosition = transform.position;
    }

    protected override void Used()
    {
        base.Used();
        StartCoroutine(Move());
        // if(!isShaking)
        //     ShakeObject();
        // if (!isOut)
        // {
        //     this.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder++;
        //     isOut= true;
        // }
        // else
        // {
        //     isOut = false;
        //     this.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder--;
        // }



    }

    IEnumerator Move()
    {
        transform.DOMoveY(-6f, 1.5f);
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
    
    void ShakeObject()
    {
        isShaking = true;
        
        // 使用Dotween实现晃动效果
        transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness)
            .OnComplete(() => 
            {
                // 晃动结束后回到原位置
                transform.position = originalPosition;
                isShaking = false;
            });
    }
    
    
    
}
