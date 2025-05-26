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
    
    void Start()
    {
        UpdateScore(0);
        UpdateCombo(0);
        timeUI.ResetTime();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScore(score);
    }

    public void IncreaseCombo()
    {
        combo++;
        UpdateCombo(combo);
    }

    public void ResetCombo()
    {
        combo = 0;
        UpdateCombo(combo);
    }

    private void UpdateScore(int newScore)
    {
        scoreText.text = newScore.ToString();
    }

    private void UpdateCombo(int newCombo)
    {
        comboText.text = newCombo.ToString();
    }

    
}
