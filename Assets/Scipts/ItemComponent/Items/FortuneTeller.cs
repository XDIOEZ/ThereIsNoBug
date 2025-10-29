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

        if (Inventory.Instance.nowItem.id == 110)
        {
            StartCoroutine(CountTime());
            return;
        }
        else if (isFirst == false)
        {
            UIMgr.Instance().GetPanel<GamePanel>("GamePanel").InitDialogBox("若对这份指引存疑，可把牌交还于我——命运的路口…你仍有重新来过的机会…",
                transform.position + new Vector3(0.5f, 1f));
            return;
        }
        else
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
            zhanbushi.isFirst = false;
        }

        
        yield return new WaitForSeconds(2f);
        //todo:跳转到卡牌游戏界面
        if (Inventory.Instance.itemInInventory.FirstOrDefault(x => x.id == 110))
        {
            Inventory.Instance.RemoveItem(110);
        }
        UIMgr.Instance().ShowPanel<BasePanel>("LittleGamePanel", E_UI_Layer.Top);
        
        GetBack();
        // this.gameObject.GetComponent<canInteract>().enabled = false;
        // this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void GetBack()
    {
        this.gameObject.GetComponent<canInteract>().enabled = true;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
