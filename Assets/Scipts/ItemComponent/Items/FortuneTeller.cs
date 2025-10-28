using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class FortuneTeller : Item
{
    InteractableComponent interactableComponent;
    private bool isFirst = true;
    
    public zhanbushi zhanbushi;
    
    private void Start()
    {
        interactableComponent = GetComponent<InteractableComponent>();
        interactableComponent.OnInteract += Used;
    }

    protected override void Used()
    {
        StartCoroutine(CountTime());

    }

    protected override IEnumerator CountTime()
    {
        if (isFirst)
        {
            UIMgr.Instance().GetPanel<GamePanel>("GamePanel").InitDialogBox("每三张牌里藏着唯一的指引，选出其中“与众不同”的…它们会帮你看清前路的方向。",transform.position + new Vector3(0.5f,1f));
            isFirst = false;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            this.gameObject.GetComponent<canInteract>().enabled = false;
            zhanbushi.flag = false;
        }
        
        yield return new WaitForSeconds(2f);
        //todo:跳转到卡牌游戏界面
        if (Inventory.Instance.itemInInventory.FirstOrDefault(x => x.id == 110))
        {
            Inventory.Instance.RemoveItem(110);
        }
        UIMgr.Instance().ShowPanel<BasePanel>("LittleGamePanel", E_UI_Layer.Top);

        this.gameObject.GetComponent<canInteract>().enabled = false;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        zhanbushi.flag = false;
    }

    public void GetBack()
    {
        this.gameObject.GetComponent<canInteract>().enabled = true;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
