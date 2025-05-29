using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    private int currentScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int score)
    {
        currentScore += score * 100;
        Debug.Log($"점수: {currentScore}");

        UIManager.Instance?.UpdateScore(currentScore);
    }

    public void AddCombo(int combo)
    {
        if (ComboSystem.Instance != null)
        {
            for (int i = 0; i < combo; i++)
            {
                ComboSystem.Instance.AddCombo();
            }
        }
        else
        {
            Debug.LogWarning("ComboSystem.Instance가 null입니다.");
        }
    }
}
