using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private int score = 1; // 획득 점수
    private int combo = 1; // 장애물 카운트
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(score);
            ScoreManager.Instance.AddCombo(combo);
        }
    }
}
