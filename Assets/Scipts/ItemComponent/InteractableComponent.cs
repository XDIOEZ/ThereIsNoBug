using System;
using UnityEngine;

public class InteractableComponent : MonoBehaviour
{
    public event Action OnInteract;
    public event Action<Item> OnInteractWithItem;

    public void Interact()
    {
        OnInteract?.Invoke();
    }
    public void Interact(Item item)
    {
        OnInteractWithItem?.Invoke(item);
    }
}
