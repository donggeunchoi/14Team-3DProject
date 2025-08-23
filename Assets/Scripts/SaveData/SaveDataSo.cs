using UnityEngine;

[CreateAssetMenu(menuName = "Config/GameConfig")]
public class SaveDataSo : ScriptableObject
{
    [Header("Default Values")] 
    [Range(0, 1)] public float defaultBGM = 1f;
    [Range(0, 1)] public float defaultSFX = 1f;

    [Header("Score")] public int defaultScore = 0;
    
    [Header("CutScene Flags")]
    public bool isCutScene = false;
}
