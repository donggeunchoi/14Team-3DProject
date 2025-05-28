using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("게임플레이음악 설정")]
    public List<AudioClip> bgmPlaylist;
    [Range(0f, 1f)] public float bgmVolume = 0.08f;
    [Tooltip("게임플레이 음악 전환 간격(초)")] public float bgmChangeInterval = 120f;
    private AudioSource bgmSource;
    private int currentBgmIndex = 0;
    private float bgmTimer = 0f;

    [Header("효과음 설정")]
    public AudioClip jumpSound;
    public AudioClip coinSound;
    public AudioClip gameOverSound;
    public AudioClip gameStartSound;
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

        // 볼륨 설정 로드
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", bgmVolume);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", sfxVolume);
    }

    private void Update()
    {
        if (isGamePlaying && bgmPlaylist.Count > 1)
        {
            UpdateBGMPlayback();
        }
    }

    public void StartGameMusic()
    {
        if (!isGamePlaying)
        {
            if (gameStartSound != null)
            {
                PlaySFX(gameStartSound);
                Invoke("PlayFirstBGM", gameStartSound.length);
            }
            else
            {
                PlayFirstBGM();
            }
            isGamePlaying = true;
        }
    }

    private void PlayFirstBGM()
    {
        if (bgmPlaylist.Count == 0) return;

        currentBgmIndex = 0;
        bgmTimer = 0f;
        bgmSource.clip = bgmPlaylist[currentBgmIndex];
        bgmSource.volume = bgmVolume;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    private void UpdateBGMPlayback()
    {
        bgmTimer += Time.deltaTime;

        if (bgmTimer >= bgmChangeInterval || !bgmSource.isPlaying)
        {
            PlayNextBGM();
        }
    }

    private void PlayNextBGM()
    {
        currentBgmIndex = (currentBgmIndex + 1) % bgmPlaylist.Count;
        bgmTimer = 0f;
        bgmSource.clip = bgmPlaylist[currentBgmIndex];
        bgmSource.Play();
    }

    public void StopGameMusic()
    {
        isGamePlaying = false;
        bgmSource.Stop();
    }

  
    public void PlayJumpSound() => PlaySFX(jumpSound);
    public void PlayCoinSound() => PlaySFX(coinSound);

    public void PlayGameOverSound()
    {
        PlaySFX(gameOverSound);
        StopGameMusic();
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }

    
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        bgmSource.volume = bgmVolume;
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }
}