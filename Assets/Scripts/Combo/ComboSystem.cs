using UnityEngine;
using TMPro;

public class ComboSystem : MonoBehaviour
{
   

    [Header("Sound Settings")]
    public AudioSource audioSource;
    public AudioClip[] comboSounds; 
    [Range(0, 1)] public float volume = 1f;

    [Header("Special Combo Sounds")]
    public AudioClip combo10Sound; // 10콤보 달성 사운드
    public AudioClip combo50Sound; // 50콤보 달성 사운드
    public AudioClip combo100Sound; // 100콤보 달성 사운드
    [Range(0, 1)] public float specialComboVolume = 1.2f; // 특수 콤보 사운드 볼륨

    [Header("Combo Settings")]
    public float comboTimeout = 2f;
    public int maxComboLevel = 100;
    public bool showPopup = true;

    private int currentCombo = 0;
    private float lastComboTime = 0f;
    private bool isComboActive = false;

    // 특수 콤보 달성 여부 추적
    private bool hasPlayed10Combo = false;
    private bool hasPlayed50Combo = false;
    private bool hasPlayed100Combo = false;

    // 콤보 변경 시 호출될 이벤트
    public delegate void ComboChangedHandler(int newCombo);
    public event ComboChangedHandler OnComboChanged;

    private void Update()
    {
        if (isComboActive && Time.time - lastComboTime > comboTimeout)
        {
            ResetCombo();
        }
    }

    // 콤보 증가
    public void AddCombo()
    {
        currentCombo++;
        lastComboTime = Time.time;
        isComboActive = true;

        
        PlayComboSound();
        PlaySpecialComboSound(); // 특수 콤보 사운드 재생
        

        OnComboChanged?.Invoke(currentCombo);
    }

    // 특수 콤보 사운드 재생
    private void PlaySpecialComboSound()
    {
        if (audioSource == null) return;

        // 10콤보 달성
        if (currentCombo == 10 && !hasPlayed10Combo && combo10Sound != null)
        {
            audioSource.PlayOneShot(combo10Sound, specialComboVolume);
            hasPlayed10Combo = true;
        }
        // 50콤보 달성
        else if (currentCombo == 50 && !hasPlayed50Combo && combo50Sound != null)
        {
            audioSource.PlayOneShot(combo50Sound, specialComboVolume);
            hasPlayed50Combo = true;
        }
        // 100콤보 달성
        else if (currentCombo == 100 && !hasPlayed100Combo && combo100Sound != null)
        {
            audioSource.PlayOneShot(combo100Sound, specialComboVolume);
            hasPlayed100Combo = true;
        }
    }

    // 콤보 리셋
    public void ResetCombo()
    {
        if (currentCombo == 0) return;

        currentCombo = 0;
        isComboActive = false;

        // 특수 콤보 플래그 리셋
        hasPlayed10Combo = false;
        hasPlayed50Combo = false;
        hasPlayed100Combo = false;

        

        OnComboChanged?.Invoke(currentCombo);
    }

    // 콤보 점수 배수 계산
    public float GetComboMultiplier()
    {
        // 기본적으로 콤보 수에 비례한 배수 (예: 1.1, 1.2, 1.3...)
        return 1f + (Mathf.Clamp(currentCombo, 0, maxComboLevel) * 0.1f);
    }

   

    private void PlayComboSound()
    {
        if (audioSource != null && comboSounds.Length > 0)
        {
            int soundIndex = Mathf.Clamp(currentCombo - 1, 0, comboSounds.Length - 1);
            audioSource.PlayOneShot(comboSounds[soundIndex], volume);
        }
    }

    

  

    // 현재 콤보 수 가져오기
    public int GetCurrentCombo() => currentCombo;

    // 콤보 활성 상태 확인
    public bool IsComboActive() => isComboActive;
}