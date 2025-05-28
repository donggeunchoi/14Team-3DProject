using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//게임 상태에 대한 enum
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
    
    //싱글톤으로 만들어놨지만 2회차 게임 시작 버튼이 작동 불가로
    //DondestroyOnLoad(gameObject)지운 상태
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            CurrentState = GameState.Waiting;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    //시작할때 멈추기
    private void Start()
    {
        Time.timeScale = 0;
        CurrentState = GameState.Waiting;
        
    }
    
    //시작버튼을 누르고 게임 진행
    public void StartGame()
    {
        CurrentState = GameState.Playing;
        Time.timeScale = 1f;
        UIManager.Instance.ResetUI();
    }
    
    //멈추기를 눌렀을때 게임 멈추기
    public void StopGame()
    {
        Time.timeScale = 0f;
        CurrentState = GameState.Stop;
    }
    
    //재시작하기를 눌렀을때 작동하는 메서드
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        CurrentState = GameState.Playing;
    }
    
    //게임 오버되었을때 게임이 멈추게 만드는 메서드
    public void GameOver()
    {
        Time.timeScale = 0f;
        CurrentState = GameState.GameOver;
    }
    
    // 점수를 리셋하고싶어요
    public void RestartGame()
    {
        Time.timeScale = 0f;
        if (UIManager.Instance != null)
        {
            Destroy(UIManager.Instance.gameObject);
            Destroy(ScoreManager.Instance.gameObject);
        }
        
        CurrentState = GameState.Waiting;
        
        SceneManager.LoadScene("Map_Asset");
    }
    
    
}
