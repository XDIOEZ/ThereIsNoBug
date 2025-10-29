using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class zhanbushi : MonoBehaviour
{
    public GameObject skeletonAnimation;
    
    public bool flag = false;

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
        
        if (Inventory.Instance.nowItem.id == 110)
        {
            this.gameObject.GetComponent<canInteract>().enabled = true;
            //flag = true;
        }
        else if(!Inventory.Instance.nowItem)
        {
            Debug.Log("cancel interact");
            this.gameObject.GetComponent<canInteract>().enabled = false;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
