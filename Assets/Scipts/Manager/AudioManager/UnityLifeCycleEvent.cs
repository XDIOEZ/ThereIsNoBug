using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityLifeCycleEvent : MonoBehaviour
{
    public UnityEvent _OnAwake;
    public UnityEvent _OnStart;
    public UnityEvent _OnEnable;
    public UnityEvent _OnDisable;
    public UnityEvent _OnDestroy;

    private void Start()
    {
        _OnStart.Invoke();
    }
}
