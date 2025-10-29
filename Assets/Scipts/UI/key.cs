using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    public bool isFirst = true;
    
    private static key instance;
    
    public static key Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<key>();
            }
            return instance;
        }
    }
    
    
    private void Update()
    {
        CheckIsPass();
        if (!isFirst)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
            isFirst = true;
        }
    }
    
    public void CheckIsPass()
    {
        // 如果音量小于等于0.01f，才能捡起钥匙
        if (AudioManager.Instance.bgmVolume <= 0.01f)
        {
            //Debug.Log("canPick");
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        else if (AudioManager.Instance.bgmVolume >= 0.01f)
        {
            //Debug.Log("canNotPick");
         //   this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    
}
