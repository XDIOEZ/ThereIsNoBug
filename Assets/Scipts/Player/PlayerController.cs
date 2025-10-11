using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerControl input;
    private Rigidbody2D rb;
    [SerializeField] private Vector2 dir;
    [SerializeField] private float speed;

    private void Awake()
    { 
        input = new PlayerControl(); 
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    { 
        input.Enable(); 
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Update()
    {
        dir = input.GamePlay.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rb.velocity = dir * speed * Time.deltaTime;
    }
}
