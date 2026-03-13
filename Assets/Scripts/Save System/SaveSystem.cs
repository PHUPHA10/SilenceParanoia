using UnityEngine;
using System.IO;

public static class SaveSystem
{
    static string savePath = Application.persistentDataPath + "/save.json";

    public static void SaveGame(Vector3 playerPos, string sceneName)
    {
        SaveData data = new SaveData();

        data.sceneName = sceneName;
        data.playerX = playerPos.x;
        data.playerY = playerPos.y;
        data.playerZ = playerPos.z;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(savePath, json);

        Debug.Log("Game Saved!");
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<SaveData>(json);
        }

        return null;
    }

    public static bool SaveExists()
    {
        return File.Exists(savePath);
    }
}