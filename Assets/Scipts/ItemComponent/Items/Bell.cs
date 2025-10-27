using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bell : Item
{
    private InteractableComponent interactableComponent;
    public UnityEvent OnInteract;

    private void Start()
    {
        interactableComponent = GetComponent<InteractableComponent>();
        interactableComponent.OnInteract += Used;
    }

    protected override void Used()
    {
        base.Used();
        OnInteract?.Invoke();
    }
}
