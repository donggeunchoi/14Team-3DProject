using UnityEngine;

public class Score : MonoBehaviour
{
    private int score = 1;
    private int combo = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && gameObject.CompareTag("Untagged"))
        {
            ScoreManager.Instance.AddScore(score);
            ScoreManager.Instance.AddCombo(combo);
        }
        else if (other.gameObject.CompareTag("Player") && gameObject.CompareTag("RollingObstacle"))
        {
            ScoreManager.Instance.AddCombo(combo);
        }
    }
}
