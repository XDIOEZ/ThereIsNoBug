using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class zhanbushi : MonoBehaviour
{
    public GameObject skeletonAnimation;
    
    public bool isFirst = true;

    private void Start()
    {
        skeletonAnimation.SetActive(false);
    }

    private void Update()
    {
        
        if (currentPosition.Instance.Y_currentindex == 1)
        {
            skeletonAnimation.SetActive(true);
        }
        else
        {
            skeletonAnimation.SetActive(false);
        }
        
        var nowItem = Inventory.Instance != null ? Inventory.Instance.nowItem : null;

        if (nowItem != null && isFirst == false)
        {
            if (nowItem.id == 110)
            {
                var interact = GetComponent<canInteract>();
                if (interact != null) interact.enabled = true;
            }
            else
            {
                var interact = GetComponent<canInteract>();
                if (interact != null) interact.enabled = false;
                var col = GetComponent<BoxCollider2D>();
                if (col != null) col.enabled = false;
            }
        }
        else if(nowItem == null && isFirst == false)
        {
            Debug.Log("cancel interact");
            var interact = GetComponent<canInteract>();
            if (interact != null) interact.enabled = false;
            var col = GetComponent<BoxCollider2D>();
            if (col != null) col.enabled = false;
        }
        
        
        // if (Inventory.Instance.nowItem.id == 110)
        // {
        //     this.gameObject.GetComponent<canInteract>().enabled = true;
        //     //flag = true;
        // }
        // else if(!Inventory.Instance.nowItem)
        // {
        //     Debug.Log("cancel interact");
        //     this.gameObject.GetComponent<canInteract>().enabled = false;
        //     this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        // }
    }
}
