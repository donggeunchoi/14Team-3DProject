using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    private Vector2 moveInput;
    public float speed = 25f;
    
    private void Update()
    {
        Move();
    }

    public void Move()
    {
        Vector3 move = new Vector3(moveInput.x, 0, 0);
        _rigidbody.MovePosition(this.transform.position + move * speed * Time.deltaTime);
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        else
        {
            moveInput = Vector2.zero;
        }
    }
}
