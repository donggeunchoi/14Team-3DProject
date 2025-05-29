using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    Rigidbody _rigidbody;
    public float itemMoveSpeed = 25f;


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
            PlayerCondition player = other.GetComponent<PlayerCondition>();
            // 테스트로 무적 100초로 설정
            player.ActivateInvincibility(100f);

            Destroy(gameObject); // 아이템 파괴
        }
    }
}