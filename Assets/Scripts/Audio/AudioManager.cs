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

        // 배경음 설정
        SetupBGM();
    }

    private void SetupBGM()
    {
        bgmSource.clip = backgroundMusic;
        bgmSource.volume = bgmVolume;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    #region 효과음 재생 메서드
    public void PlayJumpSound()
    {
        PlaySFX(jumpSound);
    }

    public void PlayCoinSound()
    {
        PlaySFX(coinSound);
    }

    public void PlayGameOverSound()
    {
        PlaySFX(gameOverSound);
        StopBGM(); // 게임오버 시 배경음 정지
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
        if (!bgmSource.isPlaying)
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
        bgmSource.Pause();
    }

    public void ResumeBGM()
    {
        bgmSource.UnPause();
    }
    #endregion

    #region 볼륨 조절
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        bgmSource.volume = bgmVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }
    #endregion
}