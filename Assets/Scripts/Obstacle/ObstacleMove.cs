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

    // Update is called once per frame
    void FixedUpdate()
    {
        _rigidbody.velocity = Vector3.back * obstacleSpeed; // 장애물 이동속도
    }

    // 플레이어와 장애물이 충돌하면 게임 정지
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerCondition playerCondition = other.gameObject.GetComponent<PlayerCondition>();
            if (!playerCondition.IsInvincible)
            {
            StartCoroutine(nameof(GameOver));
            }
        }
        Debug.Log($"[충돌 시작] 시간: {Time.time:F2}  {gameObject.name} vs {other.gameObject.name}  위치: {transform.position}");
    }

    IEnumerator GameOver()
    {   
        CharacterManager.Instance.Player.controller.Dead();
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(2.28f);
        
        Debug.Log("장애물과 충돌로 인한 게임정지");
        UIController.Instance.ShowGameOverUI();
    }
}