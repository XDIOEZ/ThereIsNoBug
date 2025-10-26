using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItem : Item
{
    InventoryComponent inventoryComponent;

    private void Start()
    {
        inventoryComponent = GetComponent<InventoryComponent>();
        inventoryComponent.OnUsed += Used;
    }

    protected override void Used()
    {
        base.Used();
        GamePlayManager.Instance.ShowMap();
        StartCoroutine(CountTime());
    }
}
