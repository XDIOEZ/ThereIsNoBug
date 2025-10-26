using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : Item
{
    private InteractableComponent InteractableComponent;

    private void Start()
    {
        InteractableComponent = GetComponent<InteractableComponent>();
        InteractableComponent.OnInteract += Used;
    }

    protected override void Used()
    {
        base.Used();
        gameObject.SetActive(false);
    }
}
