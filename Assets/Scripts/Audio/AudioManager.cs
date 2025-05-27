using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("배경음 설정")]
    public AudioClip backgroundMusic;
    [Range(0f, 1f)] public float bgmVolume = 0.08f;
    private AudioSource bgmSource;

    [Header("효과음 설정")]
    public AudioClip jumpSound;
    public AudioClip coinSound;
    public AudioClip gameOverSound;
    [Range(0f, 1f)] public float sfxVolume = 0.3f;
    private AudioSource sfxSource;

    private bool isGamePlaying = false; // 게임 플레이 상태 추적

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
            return;
        }

        // 오디오 소스 초기화
        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        // 초기 배경음 설정 (재생은 하지 않음)
        SetupBGM();
    }

    private void SetupBGM()
    {
        bgmSource.clip = backgroundMusic;
        bgmSource.volume = bgmVolume;
        bgmSource.loop = true;
        // Awake에서 바로 재생하지 않음
    }

    // 게임 시작 시 호출할 메서드
    public void StartGame()
    {
        isGamePlaying = true;
        PlayBGM();
    }

    // 게임 종료 시 호출할 메서드
    public void EndGame()
    {
        isGamePlaying = false;
        StopBGM();
    }

    #region 효과음 재생 메서드
    public void PlayJumpSound()
    {
        if (isGamePlaying) PlaySFX(jumpSound);
    }

    public void PlayCoinSound()
    {
        if (isGamePlaying) PlaySFX(coinSound);
    }

    public void PlayGameOverSound()
    {
        PlaySFX(gameOverSound);
        EndGame(); // 게임오버 시 배경음 정지
    }
    #endregion

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }

    #region 배경음 제어
    public void PlayBGM()
    {
        if (!bgmSource.isPlaying && isGamePlaying)
        {
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PauseBGM()
    {
        if (isGamePlaying) bgmSource.Pause();
    }

    public void ResumeBGM()
    {
        if (isGamePlaying) bgmSource.UnPause();
    }
    #endregion

    #region 볼륨 조절
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        bgmSource.volume = bgmVolume;

        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);

        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }
    #endregion
}