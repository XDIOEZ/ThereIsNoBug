using System;
using UnityEngine;

public class InteractableComponent : MonoBehaviour
{
    public event Action OnInteract;

    public void Interact()
    {
        OnInteract?.Invoke();
    }
}
