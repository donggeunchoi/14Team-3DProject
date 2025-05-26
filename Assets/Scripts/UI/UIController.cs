using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject stopPanel;
    
    private bool gameStarted = false;
    private bool isStopping = false;

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
    
}
