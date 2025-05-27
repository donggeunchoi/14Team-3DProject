using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
    Waiting,
    Playing,
    Stop,
    GameOver
    
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}
    public GameState CurrentState { get; private set;}
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CurrentState = GameState.Waiting;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Time.timeScale = 0;
        CurrentState = GameState.Waiting;
    }

    public void StartGame()
    {
        CurrentState = GameState.Playing;
        Time.timeScale = 1f;
        UIManager.Instance.ResetUI();
    }

    public void StopGame()
    {
        Time.timeScale = 0f;
        CurrentState = GameState.Stop;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        CurrentState = GameState.Playing;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        CurrentState = GameState.GameOver;
    }
    
    //리셋하고 본게임 씬으로 다시 불러오기
    public void RestartGame()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene("Map_Asset");
    }

    
    
}
