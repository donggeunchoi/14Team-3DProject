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
   
    public void ShowStartUI(bool active)
    {
        startPanel.SetActive(active);
    }

    public void ShowStopUI()
    {
        stopPanel.SetActive(true);
    }

    public void ShowGameOverUI()
    {
        endPanel.SetActive(true);
    }

    public void RemoveStart()
    {
        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }
    }

    public void RemoveStop()
    {
        stopPanel.SetActive(false);
    }
    
    public void OnClickStartButton()
    {
        GameManager.Instance.StartGame();
        RemoveStart();
    }

    public void OnClickStopButton()
    {
        GameManager.Instance.StopGame();
        ShowStopUI();
    }

    public void OnClickResumeButton()
    {
        RemoveStop();
        GameManager.Instance.ResumeGame();
    }

    public void OnClickRestartButton()
    {
        GameManager.Instance.RestartGame();
    }
}
