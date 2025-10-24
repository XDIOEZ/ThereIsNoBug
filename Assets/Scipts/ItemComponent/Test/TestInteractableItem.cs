using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractableItem : Item 
{
    InteractableComponent interactableComponent;
    void Start()
    {
        interactableComponent = GetComponent<InteractableComponent>();
        interactableComponent.OnInteract += Used;
        interactableComponent.OnInteractWithItem += UsedWithItem;
    }

    protected override void Used()
    {
        Debug.Log("Test");
    }

    protected override void UsedWithItem(Item item)
    {
        Debug.Log(item.GetName() + "别闹了，我不玩想旮旯给木");
        Inventory.Instance.ResetInventory();
    }
}
