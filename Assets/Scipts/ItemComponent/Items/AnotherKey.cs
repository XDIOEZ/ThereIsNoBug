using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherKey : Item
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
        Inventory.Instance.AddItem(1);
        Destroy(this.gameObject);
    }
}
