using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehavior : MonoBehaviour
{
    Rigidbody _rigidbody;
    Obstacle _obstacle;
    public float obstacleSpeed = 25f; // 장애물 이동속도
    public float knockbackForce = 500f; // 넉백 강도

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Destroy(gameObject,4f); // 생성 후 4초뒤 파괴
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        _rigidbody.velocity = Vector3.back * obstacleSpeed; // 장애물 이동방향과 속도
    }

    public void InitObstacle() // 초기화
    {
        _rigidbody.velocity = Vector3.back * obstacleSpeed; // 장애물 이동방향과 속도
        _rigidbody.angularVelocity = Vector3.zero; // 회전각도 초기화
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
                Vector3 selectedDirection;
                
                if (direction == 0)
                {
                    selectedDirection = Vector3.right; // 오른쪽으로 날림
                }
                else 
                {
                    selectedDirection = Vector3.left; // 왼쪽으로 날림
                }
                
                // 플레이어와 충돌하면 장애물이 대각선 방향으로 날아감
                Vector3 knockbackDirection = (selectedDirection + Vector3.up * 2f).normalized; 
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