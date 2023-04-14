using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public SaveData(string _name, int money_)
    {
        name_ = _name;
        this.money_ = money_;
    }

    public string name_;
    public int money_;

}
public static class SaveSystem
{
    private static string SavePath => Application.persistentDataPath + "/saves/";

    public static void Save(SaveData saveData, string saveFileName)
    {
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }

        string saveJson = JsonUtility.ToJson(saveData);

        string saveFilePath = SavePath + saveFileName + ".json";
        File.WriteAllText(saveFilePath, saveJson);
        Debug.Log("Save Success: " + saveFilePath);
    }

    public static SaveData Load(string saveFileName)
    {
        string saveFilePath = SavePath + saveFileName + ".json";

        if (!File.Exists(saveFilePath))
        {
            Debug.LogError("No such saveFile exists");
            return null;
        }

        string saveFile = File.ReadAllText(saveFilePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(saveFile);
        return saveData;
    }
}

public class SaveDataManager : MonoBehaviour
{
    public void DoSave()
    {
        SaveData saveData = new SaveData("±èÅÂ¿µ", 5000);
        SaveSystem.Save(saveData, "saveFile01");
        DoLoad();
    }
    public void DoLoad()
    {
        SaveData saveData = SaveSystem.Load("saveFile01");
        Debug.Log($"{saveData.name_},{saveData.money_}");
        
    }

}
