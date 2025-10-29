using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using Unity.VisualScripting;
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
        
        

        // if (Input.GetMouseButtonDown(0))
        // {
        //     
        //     Camera cam = Camera.main;
        //     if (!cam) return;
        //
        //     Vector3 mouseScreenPos = Input.mousePosition;
        //     mouseScreenPos.z = -cam.transform.position.z; // 转换到世界 z=0 平面
        //     Vector3 mouseWorldPos = cam.ScreenToWorldPoint(mouseScreenPos);
        //
        //     bool clicked = false;
        //     var col = GetComponent<BoxCollider2D>();
        //     if(col.enabled == false) Debug.Log(col.enabled);
        //     var sr = GetComponent<SpriteRenderer>();
        //     Debug.Log("click detected FT！！！！！！！！！！！！");
        //     if (!col)
        //     {
        //         if (sr.bounds.Contains(mouseWorldPos)) clicked = true;
        //     }
        //     else
        //     {
        //         // 兜底：基于 transform 位置的近似判断（可按需调整）
        //         Vector3 diff = mouseWorldPos - transform.position;
        //         if (Mathf.Abs(diff.x) <= 2f && Mathf.Abs(diff.y) <= 2f) clicked = true;
        //     }
        //     
        //     if (clicked && !isFirst)
        //     {
        //         Debug.Log("台词4");
        //     }
        //     
        // }
        
        // var nowItem = Inventory.Instance != null ? Inventory.Instance.nowItem : null;
        //
        // if (nowItem != null && !isFirst)
        // {
        //     if (nowItem.id == 110)
        //     {
        //         var interact = GetComponent<canInteract>();
        //         if (interact != null) interact.enabled = true;
        //     }
        //     else
        //     {
        //         var interact = GetComponent<canInteract>();
        //         if (interact != null) interact.enabled = false;
        //         var col = GetComponent<BoxCollider2D>();
        //         if (col != null) col.enabled = false;
        //     }
        // }
        // else if(!nowItem && !isFirst) 
        // {
        //     //Debug.Log("cancel interact");
        //     var interact = GetComponent<canInteract>();
        //     if (interact != null) interact.enabled = false;
        //     var col = GetComponent<BoxCollider2D>();
        //     if (col != null) col.enabled = false;
        // }
        
        
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
