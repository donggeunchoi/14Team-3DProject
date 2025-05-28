using UnityEngine;

public class RollingScore : MonoBehaviour
{
    private int score = 1; // 획득 점수

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 플레이어가 굴러서 트리거에 감지될 때 - 점수 얻음
            if (other.gameObject.CompareTag("Player"))
            {
                // 점수 증가
                ScoreManager.Instance.AddScore(score);
            }
        }
    }
}
