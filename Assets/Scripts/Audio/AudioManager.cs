using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("����� ����")]
    public AudioClip backgroundMusic;
    [Range(0f, 1f)] public float bgmVolume = 0.08f;
    private AudioSource bgmSource;

    [Header("ȿ���� ����")]
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

        // ����� �ҽ� �ʱ�ȭ
        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        // ����� ����
        SetupBGM();
    }

    private void SetupBGM()
    {
        bgmSource.clip = backgroundMusic;
        bgmSource.volume = bgmVolume;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    #region ȿ���� ��� �޼���
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
        StopBGM(); // ���ӿ��� �� ����� ����
    }
    #endregion

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }

    #region ����� ����
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

    #region ���� ����
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