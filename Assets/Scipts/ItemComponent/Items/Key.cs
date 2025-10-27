using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    private InteractableComponent InteractableComponent;

    private void Start()
    {
        InteractableComponent = GetComponent<InteractableComponent>();
        InteractableComponent.OnInteract += Used;
    }

    protected override void Used()
    {
        base.Used();
        Inventory.Instance.AddItem(111);
        Destroy(this.gameObject);
    }
}
