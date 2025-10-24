using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractableItem : Item 
{
    InteractableComponent interactableComponent;
    void Start()
    {
        interactableComponent = GetComponent<InteractableComponent>();
        interactableComponent.OnInteract += Used;
    }

    protected override void Used()
    {
        Debug.Log("Test");
    }
}
