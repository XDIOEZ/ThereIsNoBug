using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GamePlayManager : MonoSingleton<GamePlayManager> 
{
    public LayerMask interactableLayer;
    public bool isOnInventory;
    public Map map;
    public Card card;
    public VoidEventSO newGameEvent;
    public VoidEventSO moveUpEvent;
    public VoidEventSO moveDownEvent;
    public VoidEventSO moveLeftEvent;
    public VoidEventSO moveRightEvent;
    public MapPointsSO arrived;

    private void Start()
    {
        arrived.MapPoints.Clear();
        moveUpEvent.OnEventRaised += GoUp;
        moveUpEvent.OnEventRaised += map.CloseMap;
        moveDownEvent.OnEventRaised += GoDown;
        moveDownEvent.OnEventRaised += map.CloseMap;
        moveLeftEvent.OnEventRaised += GoLeft;
        moveLeftEvent.OnEventRaised += map.CloseMap;
        moveRightEvent.OnEventRaised += GoRight;
        moveRightEvent.OnEventRaised += map.CloseMap;
        newGameEvent.OnEventRaised += AddMap;
    }

    private void Update()
    {
        if (!isOnInventory)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            
                if (hit.collider != null&& hit.collider.gameObject.GetComponent<Card>())
                {
                    Debug.Log("Card");
                    ChangeCard();
                }
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

    private void AddMap()
    {
        Inventory.Instance.AddItem(101);
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

    public void ShowCard()
    {
        if (card.gameObject.activeInHierarchy)
        {
            card.CloseCard();
            Debug.Log("Card closed");
        }
        else
        {
            card.OpenCard();
            Debug.Log("Card opened");
        } 
    }

    public void ChangeCard()
    {
        card.ChangeSprite();
    }
    public void ChangeCardID(int first,int second)
    {
        card.GetID(first,second);
    }
    
    IEnumerator CountTime()
    {
        yield return new WaitForSeconds(0.1f);
        Inventory.Instance.ResetInventory();
    }
    
        
    public void GoUp()
    {
        MapPoint mapPoint = new MapPoint(SceneLoadManager.GetInstance().currentPosition,1);
        MapPoint _mapPoint = arrived.MapPoints.FirstOrDefault(mapPoint => mapPoint.pos == SceneLoadManager.GetInstance().currentPosition);
        if (arrived.MapPoints.FirstOrDefault(mapPoint => mapPoint.pos == new Vector2(SceneLoadManager.GetInstance().currentPosition.x , SceneLoadManager.GetInstance().currentPosition.y + 1 )) != null)
        {
            return;
        }
        if (_mapPoint != null)
        {
            if (!_mapPoint.dirs.Contains(1))
            {
                _mapPoint.dirs.Add(1);
            }
        }
        else
        {
            arrived.MapPoints.Add(mapPoint);
        }
    }
    public void GoDown()
    {
        MapPoint mapPoint = new MapPoint(SceneLoadManager.GetInstance().currentPosition,3);
        MapPoint _mapPoint = arrived.MapPoints.FirstOrDefault(mapPoint => mapPoint.pos == SceneLoadManager.GetInstance().currentPosition);
        if (arrived.MapPoints.FirstOrDefault(mapPoint => mapPoint.pos == new Vector2(SceneLoadManager.GetInstance().currentPosition.x , SceneLoadManager.GetInstance().currentPosition.y - 1 )) != null)
        {
            return;
        }
        if (_mapPoint != null)
        {
            if (!_mapPoint.dirs.Contains(3))
            {
                _mapPoint.dirs.Add(3);
            }
        }
        else
        {
            arrived.MapPoints.Add(mapPoint);
        }
    }
    public void GoLeft()
    {
        MapPoint mapPoint = new MapPoint(SceneLoadManager.GetInstance().currentPosition,0);
        MapPoint _mapPoint = arrived.MapPoints.FirstOrDefault(mapPoint => mapPoint.pos == SceneLoadManager.GetInstance().currentPosition);
        if (arrived.MapPoints.FirstOrDefault(mapPoint => mapPoint.pos == new Vector2(SceneLoadManager.GetInstance().currentPosition.x - 1 , SceneLoadManager.GetInstance().currentPosition.y)) != null)
        {
            return;
        }
        if (_mapPoint != null)
        {
            if (!_mapPoint.dirs.Contains(0))
            {
                _mapPoint.dirs.Add(0);
            }
        }
        else
        {
            arrived.MapPoints.Add(mapPoint);
        }
    }
    public void GoRight()
    {
        MapPoint mapPoint = new MapPoint(SceneLoadManager.GetInstance().currentPosition,2);
        MapPoint _mapPoint = arrived.MapPoints.FirstOrDefault(mapPoint => mapPoint.pos == SceneLoadManager.GetInstance().currentPosition);
        if (arrived.MapPoints.FirstOrDefault(mapPoint => mapPoint.pos == new Vector2(SceneLoadManager.GetInstance().currentPosition.x + 1, SceneLoadManager.GetInstance().currentPosition.y )) != null)
        {
            return;
        }
        if (_mapPoint != null)
        {
            if (!_mapPoint.dirs.Contains(2))
            {
                _mapPoint.dirs.Add(2);
            }
        }
        else
        {
            arrived.MapPoints.Add(mapPoint);
        }
    }
    
}
