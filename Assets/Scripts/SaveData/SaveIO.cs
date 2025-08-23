using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveIO : MonoBehaviour
{
    static readonly string PathStr = System.IO.Path.Combine(Application.persistentDataPath, "save.json");

    public static void Save(GameSaveData data)
    {
        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(PathStr, json);
#if UNITY_EDITOR
        Debug.Log($"[Save] {PathStr}\n{json}");
#endif
    }

    public static GameSaveData Load()
    {
        if (!File.Exists(PathStr)) return null;
        var json = File.ReadAllText(PathStr);
        return JsonUtility.FromJson<GameSaveData>(json);
    }
    
    public static bool Exists() => File.Exists(PathStr);
}
