using UnityEngine;
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
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }

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

    private void Start()
    {
        Time.timeScale = 0;
        CurrentState = GameState.Waiting;
    }

    public void StartGame()
    {
        CurrentState = GameState.Playing;
        Time.timeScale = 1f;

        // UI 리셋
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ResetUI();
        }

        // 게임 시작 음악 재생
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StartGameMusic();
        }
    }

    public void StopGame()
    {
        Time.timeScale = 0f;
        CurrentState = GameState.Stop;

        // 배경음악 일시정지
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PauseBGM();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        CurrentState = GameState.Playing;

        // 배경음악 재개
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ResumeBGM();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        CurrentState = GameState.GameOver;

        // 게임 오버 사운드 재생
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOverSound();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 0f;

        // 오디오 매니저 정리 (DontDestroyOnLoad 객체이므로 제거하지 않음)
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopBGM();
        }

        // UI 매니저 정리
        if (UIManager.Instance != null)
        {
            Destroy(UIManager.Instance.gameObject);
        }

        // 스코어 매니저 정리
        if (ScoreManager.Instance != null)
        {
            Destroy(ScoreManager.Instance.gameObject);
        }

        CurrentState = GameState.Waiting;
        SceneManager.LoadScene("Map_Asset");
    }

    // 플레이어 점프 시 호출 (예시)
    public void OnPlayerJump()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayJumpSound();
        }
    }

   
}