using UnityEngine;

public class ComboSystem : MonoBehaviour
{
    public static ComboSystem Instance { get; private set; }

    [Header("Sound Settings")]
    public AudioSource audioSource;
    public AudioClip[] comboSounds;
    [Range(0f, 1f)] public float volume = 1f;

    [Header("Special Combo Sounds")]
    public AudioClip combo10Sound;
    public AudioClip combo50Sound;
    public AudioClip combo100Sound;
    [Range(0f, 1f)] public float specialComboVolume = 1f;

    [Header("Combo Settings")]
    public float comboTimeout = 2f;
    public int maxComboLevel = 100;

    private int currentCombo = 0;
    private float lastComboTime = 0f;
    private bool isComboActive = false;

    private bool hasPlayed10Combo = false;
    private bool hasPlayed50Combo = false;
    private bool hasPlayed100Combo = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isComboActive && Time.time - lastComboTime > comboTimeout)
        {
            ResetCombo();
        }
    }

    public void AddCombo()
    {
        currentCombo++;
        lastComboTime = Time.time;
        isComboActive = true;

        PlayComboSound();
        PlaySpecialComboSound();
        UIManager.Instance?.UpdateCombo(currentCombo);
    }

    private void PlayComboSound()
    {
        if (audioSource != null && comboSounds.Length > 0)
        {
            int index = Mathf.Clamp(currentCombo - 1, 0, comboSounds.Length - 1);
            audioSource.PlayOneShot(comboSounds[index], volume);
        }
    }

    private void PlaySpecialComboSound()
    {
        if (audioSource == null) return;

        if (currentCombo == 10 && !hasPlayed10Combo && combo10Sound != null)
        {
            audioSource.PlayOneShot(combo10Sound, specialComboVolume);
            hasPlayed10Combo = true;
            Debug.Log(" 10콤보!");
            
        }
        else if (currentCombo == 50 && !hasPlayed50Combo && combo50Sound != null)
        {
            audioSource.PlayOneShot(combo50Sound, specialComboVolume);
            hasPlayed50Combo = true;
            Debug.Log(" 50콤보!");
        }
        else if (currentCombo == 100 && !hasPlayed100Combo && combo100Sound != null)
        {
            audioSource.PlayOneShot(combo100Sound, specialComboVolume);
            hasPlayed100Combo = true;
            Debug.Log(" 100콤보!");
            Time.timeScale = 0f;
            UIController.Instance.ShowClearUI();
            
        }
    }

    public void ResetCombo()
    {
        if (currentCombo == 0) return;

        Debug.Log($"콤보 리셋: {currentCombo} → 0");
        currentCombo = 0;
        isComboActive = false;

        hasPlayed10Combo = false;
        hasPlayed50Combo = false;
        hasPlayed100Combo = false;

        UIManager.Instance?.UpdateCombo(currentCombo);
    }

    public int GetCurrentCombo() => currentCombo;
    public bool IsComboActive() => isComboActive;
}
