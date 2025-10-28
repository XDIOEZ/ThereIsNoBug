using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour
{
    public SpriteRenderer spriteRenderer1;
    public SpriteRenderer spriteRenderer2;
    private int now;
    public int first;
    public int second;

    public void GetID(int _first, int _second)
    {
        first = _first;
        second = _second;
        now = first;
    }

    public void OpenCard()
    {
        this.gameObject.SetActive(true);
        gameObject.transform.position = Camera.main.ViewportToWorldPoint(
            new Vector3(0.75f, 0, Camera.main.nearClipPlane + 1f));
        gameObject.transform.DOMoveY(Camera.main.ViewportToWorldPoint(
            new Vector3(0.75f, 0.25f, Camera.main.nearClipPlane + 1f)).y,1);
    }
    
    public void CloseCard()
    {
        gameObject.transform.DOMoveY(Camera.main.ViewportToWorldPoint(
            new Vector3(0.75f, 0, Camera.main.nearClipPlane + 1f)).y,1);
        this.gameObject.SetActive(false);
    }
    
    public void ChangeSprite()
    {
        if (now == first)
        {
            now = second;
            spriteRenderer1.sprite = Resources.Load<Sprite>(ItemUtils.GetItemInfo(second).ImagePath);
            spriteRenderer2.sprite = Resources.Load<Sprite>(ItemUtils.GetItemInfo(first).ImagePath);
        }
        else if (now == second)
        {
            now = first;
            spriteRenderer1.sprite = Resources.Load<Sprite>(ItemUtils.GetItemInfo(first).ImagePath);
            spriteRenderer2.sprite = Resources.Load<Sprite>(ItemUtils.GetItemInfo(second).ImagePath);
        }
    }
}