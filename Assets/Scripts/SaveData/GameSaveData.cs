using System;
using System.Collections.Generic;


[Serializable]
public class GameSaveData
{
    // Volume
    public float bgmVolume;
    public float sfxVolume;

    // Score
    public int bestScore;

    // Cutscene flags
    public bool seenPrologueCutscene;
   
}
