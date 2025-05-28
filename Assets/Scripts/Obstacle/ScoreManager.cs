using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance {get; private set;} // 싱글톤
    private int currentScore = 0; // 현재 점수
    private int currnetCombo = 0; // 현재 넘은 장애물 갯수
    
    void Awake()
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
        currentScore += score * 100 ;
        Debug.Log($"점수: {currentScore}");
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScore(currentScore);
        }
        else
        {
            Debug.LogWarning("UIManager.Instance가 없어요");
        }
        
    }

    public void AddCombo(int combo)
    {
        currnetCombo += combo;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateCombo(currnetCombo);
        }
    }
}
