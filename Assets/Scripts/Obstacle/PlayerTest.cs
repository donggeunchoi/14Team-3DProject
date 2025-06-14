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
    public float moveSpeed = 25f;
    public float jumpForce = 5f;
    
    private void Update()
    {
        Move();
    }

    public void Move()
    {
        Vector3 move = new Vector3(moveInput.x, 0, 0);
        _rigidbody.MovePosition(this.transform.position + move * moveSpeed * Time.deltaTime);
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
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
