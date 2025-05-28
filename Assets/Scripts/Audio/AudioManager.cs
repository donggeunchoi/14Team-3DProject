using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour //
{
    public static AudioManager Instance { get; private set; }

    [Header("BGM Settings")]
    public List<AudioClip> bgmPlaylist;
    public AudioClip gameStartSound;
    [Range(0f, 1f)] public float bgmVolume = 0.08f;
    private AudioSource bgmSource;

    [Header("SFX Settings")]
    public AudioClip jumpSound; // 스페이스바 입력 시 재생될 점프 사운드
    public AudioClip gameOverSound;
    [Range(0f, 1f)] public float sfxVolume = 0.3f;
    private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            bgmSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();

            // 볼륨 설정 로드
            bgmVolume = PlayerPrefs.GetFloat("BGMVolume", bgmVolume);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", sfxVolume);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // 스페이스바 입력 감지
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayJumpSound();
        }
    }

    public void StartGameMusic()
    {
        // 시작 효과음 재생
        if (gameStartSound != null)
        {
            PlaySFX(gameStartSound);
            Invoke("PlayRandomBGM", gameStartSound.length);
        }
        else
        {
            PlayRandomBGM();
        }
    }

    private void PlayRandomBGM()
    {
        if (bgmPlaylist.Count == 0) return;

        int randomIndex = Random.Range(0, bgmPlaylist.Count);
        bgmSource.clip = bgmPlaylist[randomIndex];
        bgmSource.volume = bgmVolume;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PauseBGM()
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Pause();
        }
    }

    public void ResumeBGM()
    {
        if (!bgmSource.isPlaying)
        {
            bgmSource.UnPause();
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlayGameOverSound()
    {
        PlaySFX(gameOverSound);
        StopBGM();
    }

    public void PlayJumpSound()
    {
        PlaySFX(jumpSound);
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