using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemAudio : MonoBehaviour
{
    public InteractableComponent interactableComponent;
    public UnityEvent onAudioPlayed;
    public void Start()
    {
        interactableComponent.OnInteract += onAudioPlayed.Invoke;
    }
public void OnDestroy()
    {
        interactableComponent.OnInteract -= onAudioPlayed.Invoke;
    }

}
