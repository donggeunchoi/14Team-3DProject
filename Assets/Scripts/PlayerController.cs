using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("MovementSetting")]
    public float moveSpeed; //플레이어 기본 전진 속도 앞으로
    public float laneChangeSpeed; //좌우 이동 속도
    public float jumpForce; // 점프력
    public float laneDistance; //레인 간 거리
    private int laneCount = 3;
    private int currentLaneIndex = 0;  //-1,0,1 *landDistance해서 배치
    public LayerMask groundLayerMask;

    
    
    [HideInInspector] public bool canLook = true;

    private Rigidbody _rigidbody;
    

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        MoveForward();
        UpdateLanePosition();
    }

    // private void LateUpdate()
    // {
    //     if (canLook)
    //     {
    //         // CameraLook();
    //     }
    // }


    private void MoveForward()
    {
        Vector3 forwardMovement = Vector3.forward * moveSpeed * Time.deltaTime;
        _rigidbody.MovePosition(_rigidbody.position + forwardMovement);
    }

    private void UpdateLanePosition()
    {
        float targetX = (currentLaneIndex) * laneDistance;
        Vector3 targetPosition = new Vector3(targetX, _rigidbody.position.y, _rigidbody.position.z)
            + Vector3.forward * moveSpeed*Time.deltaTime;
        
        Vector3 newPosition = Vector3.Lerp(_rigidbody.position, targetPosition,laneChangeSpeed*Time.deltaTime);
        _rigidbody.MovePosition(newPosition);
    }

    
    
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnMoveRightInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && currentLaneIndex < 1)
        {
            currentLaneIndex++;
        }
    }

    public void OnMoveLeftInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && currentLaneIndex > -1)
        {
            currentLaneIndex--;
        }
    }

   
    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (-transform.up * 0.2f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (-transform.up * 0.2f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (-transform.up * 0.2f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (-transform.up * 0.2f), Vector3.down)
        };
        for(int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }
}
