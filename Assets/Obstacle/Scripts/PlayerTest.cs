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
    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] private float jumpForce = 10f;
    
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 move = new Vector3(moveInput.x, 0, 0);
        _rigidbody.MovePosition(this.transform.position + move * moveSpeed * Time.deltaTime);
    }
    
    private void OnMove(InputAction.CallbackContext context)
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
    
    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
