using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}
    
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
    
    //리셋하고 본게임 씬으로 다시 불러오기
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Map_Asset");
        UIManager.Instance.ResetUI();

    }
    
}
