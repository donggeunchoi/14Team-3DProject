using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("Movement Setting")]
    public float moveSpeed; //플레이어 기본 전진 속도 앞으로
    public float laneChangeSpeed; //좌우 이동 속도
    public float jumpForce; // 점프력
    public float laneDistance; //레인 간 거리
    public LayerMask groundLayerMask;
    
    private int _laneCount = 3;
    private int _currentLaneIndex = 0;  //-1,0,1 *landDistance해서 배치

    [Header("Roll Setting")]
    public float rollDuration; // 구르기 지속시간
    public float slideColliderHeight; // 굴렀을 때 콜라이더 높이
    public float slideColliderCenterY; // 굴렀을때 콜라이더 y좌표

    private bool isRolling = false;
    private float _originalColldierHeight;
    private Vector3 _originalColliderCenter;
    private CapsuleCollider _capsuleCollider;
    private Animator _animator;
    [HideInInspector] public bool canLook = true;

    private Rigidbody _rigidbody;
    

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _animator = GetComponentInChildren<Animator>();
        _originalColldierHeight = _capsuleCollider.height;
        _originalColliderCenter = _capsuleCollider.center;
        
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
        Vector3 forwardMovement = Vector3.forward * (moveSpeed * Time.deltaTime);
        _rigidbody.MovePosition(_rigidbody.position + forwardMovement);
    }

    private void UpdateLanePosition()
    {
        float targetX = (_currentLaneIndex) * laneDistance;
        Vector3 targetPosition = new Vector3(targetX, _rigidbody.position.y, _rigidbody.position.z)
            + Vector3.forward * (moveSpeed * Time.deltaTime);
        
        Vector3 newPosition = Vector3.Lerp(_rigidbody.position, targetPosition,laneChangeSpeed*Time.deltaTime);
        _rigidbody.MovePosition(newPosition);
    }

    
    
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            _animator.SetTrigger("Jump");
        }
    }

    public void OnMoveRightInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && _currentLaneIndex < 1)
        {
            _currentLaneIndex++;
        }
    }

    public void OnMoveLeftInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && _currentLaneIndex > -1)
        {
            _currentLaneIndex--;
        }
    }
    
    public void onRollInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded() && isRolling == false)
        {
            Roll();
            _animator.SetTrigger("Roll");
        }
    }
   
    bool IsGrounded()
    {
        float checkDistance = _capsuleCollider.radius + 0.1f;
        Vector3 origin = transform.position
            +_capsuleCollider.center
            -Vector3.up *(_capsuleCollider.height / 2 - _capsuleCollider.radius);
        
        return Physics.Raycast(origin,Vector3.down,checkDistance,groundLayerMask);
    }

    private IEnumerator Roll()
    {
        isRolling = true;
        
        _capsuleCollider.height  = slideColliderHeight;
        _capsuleCollider.center  = new Vector3(_originalColliderCenter.x, slideColliderCenterY, _originalColliderCenter.z);
        yield return new WaitForSeconds(rollDuration);
        
        _capsuleCollider.height = _originalColldierHeight;
        _capsuleCollider.center = _originalColliderCenter;
        isRolling = false;
    }
}
