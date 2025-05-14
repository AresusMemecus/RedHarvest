using UnityEngine;

public static class ReplicaLoader
{
    public static ReplicaData LoadReplica(string filePath)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(filePath);
        if (jsonFile == null)
        {
            Debug.LogError($"Файл {filePath}.json не найден в Resources.");
            return null;
        }
        return JsonUtility.FromJson<ReplicaData>(jsonFile.text);
    }
}
