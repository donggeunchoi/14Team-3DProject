using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    Rigidbody _rigidbody;
    public float itemMoveSpeed = 25f; // 아이템 이동 속도

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rigidbody.velocity = Vector3.back * itemMoveSpeed; // 장애물 이동속도
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 충돌한 플레이어의 PlayerCondition 컴포넌트를 가져옴
            PlayerCondition player = other.GetComponent<PlayerCondition>();
            player.ActivateInvincibility(10f); // 10초간 무적

            if (ItemSpawner.Instance != null)
            {
                ItemSpawner.Instance.ReturnItemToPool(gameObject); // 이 아이템 객체를 풀로 반환
            }
        }
    }
}