using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardItem : Item
{
    public int first ;
    public int second;
    InventoryComponent inventoryComponent;

    private void Start()
    {
        inventoryComponent = GetComponent<InventoryComponent>();
        inventoryComponent.OnUsed += ShowCard;
    }

    public void SetCard(int first, int second)
    {
        this.first = first;
        this.second = second;
    }

    public void ShowCard()
    {
        GamePlayManager.Instance.ShowCard();
        GamePlayManager.Instance.ChangeCardID(first,second);
        GamePlayManager.Instance.ChangeCard();
        StartCoroutine(CountTime());
    }
}
