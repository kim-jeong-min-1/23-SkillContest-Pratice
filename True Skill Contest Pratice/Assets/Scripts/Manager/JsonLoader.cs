using UnityEngine;
using System.IO;

public class JsonLoader : MonoBehaviour
{
    private static readonly string savePath = Application.persistentDataPath + "/";

    public static T Load<T>(string saveName) where T : new()
    {
        var path = savePath + saveName + ".json";

        if (!File.Exists(path))
        {
            T t = new T();
            Save(t, saveName);
            return t;
        }

        string data = File.ReadAllText(path); 
        return JsonUtility.FromJson<T>(data);
    }

    public static void Save<T>(T data, string saveName) where T : new()
    {
        var path = savePath + saveName + ".json";

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        string file = JsonUtility.ToJson(data);
        File.WriteAllText(path, file);
    }

}
