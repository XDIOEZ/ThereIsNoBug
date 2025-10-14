using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycastInteraction : MonoBehaviour
{
    [Header("Settings")]
    public float interactionDistance = 3f;
    public LayerMask interactableLayer;
    
    [Header("Debug")]
    public bool showDebugRay = true;
    public Color rayColor = Color.green;
    
    [SerializeField]private Vector2 facingDir = Vector2.up;

    public void TryInteract()
    {
        Vector2 rayOriginPos = transform.position;
        facingDir = GetComponent<PlayerController>().GetDir();
        
        RaycastHit2D hit = Physics2D.Raycast(rayOriginPos, facingDir, interactionDistance, interactableLayer);
        
        if (showDebugRay)
        {
            Debug.DrawRay(rayOriginPos, facingDir * interactionDistance, rayColor, 1f);
        }
        
        if (hit.collider != null)
        {
            InteractableComponent interactable = hit.collider.GetComponent<InteractableComponent>();
            if (interactable != null)
            {
                interactable.Interact();
            }
            else
            {
                Debug.Log("No Interactable Component");
            }
        }
        else
        {
            Debug.Log("No Cllider");
        }
    }
    
    // 在编辑器中可视化射线
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, facingDir * interactionDistance);
    }
}
