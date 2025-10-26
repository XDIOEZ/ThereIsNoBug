using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : Item
{
    InteractableComponent interactableComponent;

    private void Start()
    {
        interactableComponent = GetComponent<InteractableComponent>();
        interactableComponent.OnInteract += Used;
    }

    protected override void Used()
    {
        base.Used();
    }
}
]