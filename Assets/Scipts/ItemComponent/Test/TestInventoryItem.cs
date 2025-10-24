using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInventoryItem : Item
{
    InventoryComponent inventoryComponent;

    private void Start()
    {
        inventoryComponent = GetComponent<InventoryComponent>();
        inventoryComponent.OnUsed += Used;
    }

    protected override void Used()
    {
        Debug.Log(name + " " + id);
    }
}
