using UnityEngine;
using System;
public class InventoryComponent : MonoBehaviour
{
    public event Action OnUsed;
    public int index;
    
    public void Interact()
    {
        OnUsed?.Invoke();
    }
}
