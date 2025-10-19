using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoSingleton<GamePlayManager> 
{
    public LayerMask interactableLayer;
    public Inventory inventory;
    public bool isOnInventory;
    
    
    private void Update()
    {
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
        }
        else
        {
            
        }
    }
}
