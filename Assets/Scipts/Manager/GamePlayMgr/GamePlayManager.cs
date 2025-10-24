using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoSingleton<GamePlayManager> 
{
    public LayerMask interactableLayer;
    public bool isOnInventory;
    public Map map;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //test
            Inventory.Instance.AddItem(1);
            Inventory.Instance.AddItem(2);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            //test
            Inventory.Instance.RemoveItem(1);
        }
        if (!isOnInventory)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero,interactableLayer);
            
                if (hit.collider != null)
                {
                    hit.collider.gameObject.GetComponent<InteractableComponent>().Interact();
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (map.gameObject.activeInHierarchy)
                {
                    map.CloseMap();
                }
                else
                {
                    map.OpenMap();
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero,interactableLayer);
            
                if (hit.collider != null)
                {
                    hit.collider.gameObject.GetComponent<InteractableComponent>().Interact(Inventory.Instance.nowItem);
                }
            }
        }
    }
}
