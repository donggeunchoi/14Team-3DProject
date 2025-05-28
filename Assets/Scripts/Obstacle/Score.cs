using UnityEngine;

public class Score : MonoBehaviour
{
    private int score = 1; // 획득 점수
    private int combo = 1; // 장애물 카운트

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 태그가 플레이어고 해당 오브젝트의 태그가 언태그일 때
        if (other.gameObject.CompareTag("Player") && gameObject.CompareTag("Untagged"))
        {
            // 점수, 콤보 둘 다 증가
            ScoreManager.Instance.AddScore(score);
            ScoreManager.Instance.AddCombo(combo);
        }
        // 충돌한 태그가 플레이어고 해당 오브젝트의 태그가 RollingObstacle일 때
        else if (other.gameObject.CompareTag("Player") && gameObject.CompareTag("RollingObstacle"))
        {
            // 콤보만 오름 - 점수는 RollingScore.cs에서 얻음
            ScoreManager.Instance.AddCombo(combo);
        }
    }
}