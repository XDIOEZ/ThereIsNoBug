using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    private void Update()
    {
        CheckIsPass();
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
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    
}
