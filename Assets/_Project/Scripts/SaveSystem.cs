using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string Path => Application.persistentDataPath + "/player.json";

    public static void Save(PlayerData data)
        => File.WriteAllText(Path, JsonUtility.ToJson(data, true));

    public static PlayerData Load()
        => File.Exists(Path) ? JsonUtility.FromJson<PlayerData>(File.ReadAllText(Path)) : null;
}
