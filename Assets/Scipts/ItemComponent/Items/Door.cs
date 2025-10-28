using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Item
{
    public bool isFirstKey;
    public bool isSecondKey;

    public GameSceneSO nextScene;
    
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
        if (item.id == 111 || item.id == 112)
        {
            if (isFirstKey == false)
            {
                isFirstKey = true;
                Inventory.Instance.RemoveItem(item.id);
                return;
            }
            isSecondKey = true;
            Inventory.Instance.RemoveItem(item.id);
        }
        
        if (isFirstKey && isSecondKey)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Art/OpenedDoor");
        }
    }

    protected override void Used()
    {
        base.Used();
        if (isFirstKey && isSecondKey)
        {
            Debug.Log("Game Pass");
            GamePlayManager.Instance.isGameOver = true;
            SceneLoadManager.GetInstance().LoadScene(nextScene,new Vector3(0,0,0),true);
            UIMgr.Instance().HidePanel("GamePanel");
        }
    }
}
