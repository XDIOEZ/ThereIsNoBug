using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bell : Item
{
    private InteractableComponent interactableComponent;
    public UnityEvent OnInteract;
    
    public GameObject skeletonPrefab;

    private void Start()
    {
        interactableComponent = GetComponent<InteractableComponent>();
        interactableComponent.OnInteract += Used;
    }

    public void Update()
    {
        if(currentPosition.Instance.Y_currentindex!=3)
        {
            skeletonPrefab.SetActive(false);
        }
        else
        {
            skeletonPrefab.SetActive(true);
        }
    }


    protected override void Used()
    {
        base.Used();
        OnInteract?.Invoke();
    }

    private void Click()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Art/Bell");
        StartCoroutine(CountTime());
    }

    protected override IEnumerator CountTime()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
