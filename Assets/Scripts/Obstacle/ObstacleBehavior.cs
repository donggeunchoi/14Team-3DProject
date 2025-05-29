using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehavior : MonoBehaviour
{
    Rigidbody _rigidbody;
    Obstacle _obstacle;
    public float obstacleSpeed = 25f;
    public float knockbackForce = 500f;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Destroy(gameObject,4f);
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        _rigidbody.velocity = Vector3.back * obstacleSpeed; // 장애물 이동속도
    }

    public void InitObstacle()
    {
        _rigidbody.velocity = Vector3.back * obstacleSpeed;
        _rigidbody.angularVelocity = Vector3.zero;
    }
    
    // 플레이어와 장애물이 충돌하면 게임 정지
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerCondition playerCondition = other.gameObject.GetComponent<PlayerCondition>();
            if (playerCondition.IsInvincible == false)
            {
                StartCoroutine(nameof(GameOver));
            }
            else
            {
                int direction = Random.Range(0, 2);
                Vector3 selectedDirection;
                
                if (direction == 0)
                {
                    selectedDirection = Vector3.right;
                }
                else
                {
                    selectedDirection = Vector3.left;
                }
                
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