using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class areaChangeListener : MonoBehaviour
{
    [Header("事件监听")] 
    public VoidEventSO OnAreaUp;


    private void OnEnable()
    {
        OnAreaUp.OnEventRaised += HandleAreaUp;
    }
    private void OnDisable()
    {
        OnAreaUp.OnEventRaised -= HandleAreaUp;
    }
    
    private void HandleAreaUp()
    {
        Debug.Log("Area Up Event Received!");
    }
    
}
