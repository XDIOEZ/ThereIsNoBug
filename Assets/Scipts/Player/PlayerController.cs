using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerControl input;
    private Rigidbody2D rb;
    [SerializeField] private Vector2 dir;
    public Vector2 lastDir;
    [SerializeField] private float speed;
    [SerializeField] bool isMoving;

    private void Awake()
    { 
        input = new PlayerControl(); 
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        input.Enable();  
        input.GamePlay.Interact.performed += Interact; 
    }
    
    private void Start()
    {

    }
    

    private void OnDisable()
    {
        input.GamePlay.Interact.performed -= Interact;
        input.Disable();
    }

    private void Update()
    {
        dir = input.GamePlay.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (dir != Vector2.zero)
        {
            Move(); 
        }
        else
        {
            rb.velocity =  Vector2.zero; 
        }
    }

    private void Move()
    {
        rb.velocity = dir * speed * Time.deltaTime;
        lastDir = dir;
    }

    public Vector2 GetDir()
    {
        return lastDir;
    }

    public void Interact(InputAction.CallbackContext cts)
    {
        GetComponent<PlayerRaycastInteraction>().TryInteract();
        
    }
    
}
