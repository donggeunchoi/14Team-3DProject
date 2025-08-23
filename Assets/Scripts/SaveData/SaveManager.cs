// SaveManager.cs
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    [SerializeField] private SaveDataSo config; // 옵션. 없으면 null 허용

    public GameSaveData Data { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 1) SO 기본값으로 초기화
        Data = new GameSaveData()
        {
            bgmVolume = config ? config.defaultBGM : 1f,
            sfxVolume = config ? config.defaultSFX : 1f,
            bestScore = config ? config.defaultScore : 0,
            seenPrologueCutscene = config ? config.isCutScene : false
        };

        // 2) 저장 파일 있으면 덮어쓰기
        var loaded = SaveIO.Load();
        if (loaded != null) Data = loaded;

        // 3) 시스템 반영 (예: 오디오)
        AudioManager.Instance?.SetBGMVolume(Data.bgmVolume);
        AudioManager.Instance?.SetSFXVolume(Data.sfxVolume);
    }

    public void SaveNow()
    {
        SaveIO.Save(Data);
    }
}