using UnityEngine;
using System;
public class InventoryComponent : MonoBehaviour
{
    public event Action OnUsed;

    public void Interact()
    {
        OnUsed?.Invoke();
    }
}
