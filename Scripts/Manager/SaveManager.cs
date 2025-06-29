using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class SaveManager : Singleton<SaveManager>
{
    private string filePath() 
    {
        return Path.Combine(Application.persistentDataPath, "save.json");
    }

    public void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath(), json);
        Debug.Log("저장 완료: " + filePath());
    }

    public GameData Load()
    {
        if (File.Exists(filePath()))
        {
            string json = File.ReadAllText(filePath());
            return JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            GameData _data = new GameData();
            _data.lisStar = new List<int>();

            Save(_data);
            return _data;
        }
    }

    public void Delete()
    {
        if (File.Exists(filePath()))
        {
            File.Delete(filePath());
            Debug.Log("저장 파일 삭제됨");
        }
    }
}


[System.Serializable]
public class GameData
{
    public List<int> lisStar;
}
