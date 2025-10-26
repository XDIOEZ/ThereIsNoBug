using System;
using System.Collections;
using System.Collections.Generic;
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
    }
}
