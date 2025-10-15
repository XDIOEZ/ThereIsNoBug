using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO targetScene;
    public Vector3 targetPosition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Teleport Enter!");
        loadEventSO.RaiseEvent(targetScene, targetPosition, true);
    }
}
