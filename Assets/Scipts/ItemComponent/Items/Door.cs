using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Item
{
    public bool isFirstKey;
    public bool isSecondKey;

    InteractableComponent interactableComponent;
    
    private void Start()
    {
        interactableComponent = GetComponent<InteractableComponent>();
        interactableComponent.OnInteract += Used;
        interactableComponent.OnInteractWithItem += UsedWithItem;
    }

    protected override void UsedWithItem(Item item)
    {
        base.UsedWithItem(item);
        if (item.id == 1)
        {
            isFirstKey = true;
        }

        if (item.id == 4)
        {
            isSecondKey = true;
        }

        if (isFirstKey && isSecondKey)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("TestArt/saki");
        }
    }

    protected override void Used()
    {
        base.Used();
        if (isFirstKey && isSecondKey)
        {
            Debug.Log("Game Pass");
        }
    }
}
