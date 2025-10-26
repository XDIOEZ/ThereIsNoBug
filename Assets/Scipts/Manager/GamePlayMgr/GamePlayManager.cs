using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoSingleton<GamePlayManager> 
{
    public LayerMask interactableLayer;
    public bool isOnInventory;
    public Map map;
    public VoidEventSO newGameEvent;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //test
            Inventory.Instance.AddItem(2);
        }
        if (!isOnInventory)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            
                //Debug.Log(hit.collider.name);
                if (hit.collider != null&& hit.collider.gameObject.GetComponent<InteractableComponent>())
                {
                    Debug.Log("interactable");
                    hit.collider.gameObject.GetComponent<InteractableComponent>().Interact();
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
                StartCoroutine(CountTime());
            }
        }
    }

    public void ShowMap()
    {
        if (map.gameObject.activeInHierarchy)
        {
            map.CloseMap();
            Debug.Log("Map closed");
        }
        else
        {
            map.OpenMap();
            Debug.Log("Map opened");
        } 
    }
    IEnumerator CountTime()
    {
        yield return new WaitForSeconds(0.1f);
        Inventory.Instance.ResetInventory();
    }
}
