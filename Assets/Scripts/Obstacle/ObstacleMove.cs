using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    Rigidbody _rigidbody;
    public float obstacleSpeed = 25f;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.position.z <= -15)
        {
            Destroy(this.gameObject);
        }
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        _rigidbody.velocity = Vector3.back * obstacleSpeed; // 장애물 이동속도
    }
    
    // 플레이어와 장애물이 충돌하면 게임 정지
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Time.timeScale = 0;
            Debug.Log("장애물과 충돌로 인한 게임정지");
            UIController.Instance.ShowGameOverUI();
        }
    }
}