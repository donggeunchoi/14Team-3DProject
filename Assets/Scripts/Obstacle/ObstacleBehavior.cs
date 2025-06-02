using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehavior : MonoBehaviour
{
    Rigidbody _rigidbody;
    public float obstacleSpeed = 15f; // 장애물 이동속도
    public float knockbackForce = 500f; // 넉백 강도

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = Vector3.back * obstacleSpeed; // 장애물 이동방향과 속도 
    }

    /// <summary>
    /// 무적으로 상태에서 충돌되어 날아가는 장애물의 transform이 기본값과 다름.
    /// 재소환 시에 기본값으로 초기화
    /// </summary>
    public void InitObstacle()
    {
        transform.rotation = Quaternion.Euler(0, 90f, 0);
        _rigidbody.velocity = Vector3.back * obstacleSpeed; // 장애물 이동방향과 속도
        _rigidbody.angularVelocity = Vector3.zero; // 회전각도 초기화
        // 장애물 모델링이 기본적으로 90도 틀어져있기 때문에 90도를 더 돌려 180도(정면)로 맞춰준다. 
    }

    // 플레이어와 장애물이 충돌하면 게임 정지
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")) // 플레이어와 충돌하면
        {
            // 충돌한 상대방 오브젝트에서 PlayerCondition이라는 스크립트 컴포넌트를 가져와라
            PlayerCondition playerCondition = other.gameObject.GetComponent<PlayerCondition>();
            if (playerCondition.IsInvincible == false) // 무적이 아닐 때
            {
                StartCoroutine(nameof(GameOver)); // 게임 오버
            }
            else // 무적일 때
            {
                int direction = Random.Range(0, 2); // 장애물 날아가는 방향 정하기
                // 왼쪽 방향과 오른쪽 방향중 하나 랜덤으로 선택됨
                Vector3 selectedDirection = (direction == 0 ? Vector3.right : Vector3.left);

                // 플레이어와 충돌하면 장애물이 대각선 방향으로 날아감
                Vector3 knockbackDirection = (selectedDirection * 5f + Vector3.up * 5f + Vector3.back).normalized;
                // Impulse 모드는 즉각적인 힘을 가함
                _rigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            }
        }
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