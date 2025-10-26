using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : Item
{
    private InteractableComponent interactableComponent;

    private void Start()
    {
        interactableComponent = GetComponent<InteractableComponent>();
        interactableComponent.OnInteract += Used;
    }

    protected override void Used()
    {
        base.Used();
        Instantiate(Resources.Load<GameObject>("Prefabs/InventoryItem/KeyItem"));
    }
}
