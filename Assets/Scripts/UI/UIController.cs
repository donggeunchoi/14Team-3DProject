using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject stopPanel;
    public GameObject endPanel;
    
    private bool gameStarted = false;
    private bool isStopping = false;
    private bool gameOver = false;
    
    public static UIController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public void OnClickStart()
    {
        if (gameStarted) return;
        
        gameStarted = true;
        startPanel.SetActive(false);
        Time.timeScale = 1f;
        
    }

    private void Start()
    {
        Time.timeScale = 0f;
        stopPanel.SetActive(false);
    }

    public void OnClickStop()
    {
        Time.timeScale = 0f;
        isStopping = true;
        stopPanel.SetActive(true);
    }

    public void OnClickRestart()
    {
        Time.timeScale = 1f;
        isStopping = false;
        stopPanel.SetActive(false);
    }
    
    public void OnClickExit()
    {
       //시작화면으로 돌아가기
       
    }

    public void Reset()
    {
        
    }

    public void GameOver()
    {
        gameOver = true;
        endPanel.SetActive(true);
    }
    
}
