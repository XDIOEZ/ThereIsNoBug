using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FortuneTeller : Item
{
    InteractableComponent interactableComponent;
    private void Start()
    {
        interactableComponent = GetComponent<InteractableComponent>();
        interactableComponent.OnInteract += Used;
    }

    protected override void Used()
    {
        //todo:跳转到卡牌游戏界面
        if (Inventory.Instance.itemInInventory.FirstOrDefault(x => x.id == 110))
        {
            Inventory.Instance.RemoveItem(110);
        }
        UIMgr.Instance().ShowPanel<BasePanel>("LittleGamePanel", E_UI_Layer.Top);
    }
}
