using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
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
            new Vector3(0.75f, 0.25f, Camera.main.nearClipPlane + 1f));
    }
    
    public void CloseCard()
    {
        this.gameObject.SetActive(false);
    }
    
    public void ChangeSprite()
    {
        if (now == first)
        {
            now = second;
            spriteRenderer.sprite = Resources.Load<Sprite>(ItemUtils.GetItemInfo(second).ImagePath);
        }
        else if (now == second)
        {
            now = first;
            spriteRenderer.sprite = Resources.Load<Sprite>(ItemUtils.GetItemInfo(first).ImagePath);
        }
    }
}