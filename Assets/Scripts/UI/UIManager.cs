using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [Header("Combo & Score")]
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI scoreText;
  

    [Header("Time")] 
    public TimeUI timeUI;

    private int score = 0;
    private int combo = 0;
  

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    //시작할때 초기화 시키기
    void Start()
    {
        UpdateScore(0);
        UpdateCombo(0);
        timeUI.ResetTime();
    }

    //업데이트 점수와 콤보
    public void UpdateScore(int newScore)
    {
        scoreText.text = newScore.ToString();
    }

    public void UpdateCombo(int newCombo)
    {
        comboText.text = newCombo.ToString();
    }
    
    //초기화 시키는 메서드 
    public void ResetUI()
    {
        score = 0;
        combo = 0;
        
        UpdateCombo(combo);
        UpdateScore(score);

        if (timeUI != null)
        {
            timeUI.ResetTime();
        }
        
        Debug.Log("리셋 완료");
    }

  

}
