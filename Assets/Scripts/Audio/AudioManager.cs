using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("배경음 설정")]
    public List<AudioClip> bgmPlaylist; // 여러 개의 배경음악 리스트
    [Range(0f, 1f)] public float bgmVolume = 0.08f;
    [Tooltip("배경음 전환 간격(초)")] public float bgmChangeInterval = 120f;
    private AudioSource bgmSource;
    private int currentBgmIndex = 0;
    private float bgmTimer = 0f;

    [Header("효과음 설정")]
    public AudioClip jumpSound;
    public AudioClip coinSound;
    public AudioClip gameOverSound;
    [Range(0f, 1f)] public float sfxVolume = 0.3f;
    private AudioSource sfxSource;

    private bool isGamePlaying = false;

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

        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (isGamePlaying && bgmPlaylist.Count > 1)
        {
            UpdateBGMPlayback();
        }
    }

    private void UpdateBGMPlayback()
    {
        bgmTimer += Time.deltaTime;

        // 현재 음악 재생 시간 체크
        if (bgmTimer >= bgmChangeInterval || !bgmSource.isPlaying)
        {
            PlayNextBGM();
        }
    }

    public void StartGame()
    {
        isGamePlaying = true;
        currentBgmIndex = 0;
        bgmTimer = 0f;
        PlayBGM(bgmPlaylist[currentBgmIndex]);
    }

    public void EndGame()
    {
        isGamePlaying = false;
        StopBGM();
    }

    private void PlayNextBGM()
    {
        currentBgmIndex = (currentBgmIndex + 1) % bgmPlaylist.Count;
        bgmTimer = 0f;
        PlayBGM(bgmPlaylist[currentBgmIndex]);
    }

    private void PlayBGM(AudioClip clip)
    {
        if (clip == null || !isGamePlaying) return;

        bgmSource.clip = clip;
        bgmSource.volume = bgmVolume;
        bgmSource.loop = true;
        bgmSource.Play();
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
        EndGame();
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

    #region 상황별 음악 전환 (선택적)
    public void ChangeBGMForSituation(int situationIndex)
    {
        if (situationIndex >= 0 && situationIndex < bgmPlaylist.Count)
        {
            currentBgmIndex = situationIndex;
            PlayBGM(bgmPlaylist[currentBgmIndex]);
        }
    }
    #endregion
}