using UnityEngine;
using TMPro;

public class ComboSystem : MonoBehaviour
{
   

    [Header("Sound Settings")]
    public AudioSource audioSource;
    public AudioClip[] comboSounds; 
    [Range(0, 1)] public float volume = 1f;

    [Header("Special Combo Sounds")]
    public AudioClip combo10Sound; // 10�޺� �޼� ����
    public AudioClip combo50Sound; // 50�޺� �޼� ����
    public AudioClip combo100Sound; // 100�޺� �޼� ����
    [Range(0, 1)] public float specialComboVolume = 1.2f; // Ư�� �޺� ���� ����

    [Header("Combo Settings")]
    public float comboTimeout = 2f;
    public int maxComboLevel = 100;
    public bool showPopup = true;

    private int currentCombo = 0;
    private float lastComboTime = 0f;
    private bool isComboActive = false;

    // Ư�� �޺� �޼� ���� ����
    private bool hasPlayed10Combo = false;
    private bool hasPlayed50Combo = false;
    private bool hasPlayed100Combo = false;

    // �޺� ���� �� ȣ��� �̺�Ʈ
    public delegate void ComboChangedHandler(int newCombo);
    public event ComboChangedHandler OnComboChanged;

    private void Update()
    {
        if (isComboActive && Time.time - lastComboTime > comboTimeout)
        {
            ResetCombo();
        }
    }

    // �޺� ����
    public void AddCombo()
    {
        currentCombo++;
        lastComboTime = Time.time;
        isComboActive = true;

        
        PlayComboSound();
        PlaySpecialComboSound(); // Ư�� �޺� ���� ���
        

        OnComboChanged?.Invoke(currentCombo);
    }

    // Ư�� �޺� ���� ���
    private void PlaySpecialComboSound()
    {
        if (audioSource == null) return;

        // 10�޺� �޼�
        if (currentCombo == 10 && !hasPlayed10Combo && combo10Sound != null)
        {
            audioSource.PlayOneShot(combo10Sound, specialComboVolume);
            hasPlayed10Combo = true;
        }
        // 50�޺� �޼�
        else if (currentCombo == 50 && !hasPlayed50Combo && combo50Sound != null)
        {
            audioSource.PlayOneShot(combo50Sound, specialComboVolume);
            hasPlayed50Combo = true;
        }
        // 100�޺� �޼�
        else if (currentCombo == 100 && !hasPlayed100Combo && combo100Sound != null)
        {
            audioSource.PlayOneShot(combo100Sound, specialComboVolume);
            hasPlayed100Combo = true;
        }
    }

    // �޺� ����
    public void ResetCombo()
    {
        if (currentCombo == 0) return;

        currentCombo = 0;
        isComboActive = false;

        // Ư�� �޺� �÷��� ����
        hasPlayed10Combo = false;
        hasPlayed50Combo = false;
        hasPlayed100Combo = false;

        

        OnComboChanged?.Invoke(currentCombo);
    }

    // �޺� ���� ��� ���
    public float GetComboMultiplier()
    {
        // �⺻������ �޺� ���� ����� ��� (��: 1.1, 1.2, 1.3...)
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

    

  

    // ���� �޺� �� ��������
    public int GetCurrentCombo() => currentCombo;

    // �޺� Ȱ�� ���� Ȯ��
    public bool IsComboActive() => isComboActive;
}