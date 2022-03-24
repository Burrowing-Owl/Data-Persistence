using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private string prevName;
    private int prevScore;
    private string thisName;

    [System.Serializable]
class SaveData
    {
        public string name;
        public int highScore;

    }

    public void ReadStringInput(string s)
    {
        thisName = s;
        Debug.Log($"Player provided {thisName} as name");

        int score = MainManager.m_Points;
        RecordNewScore(thisName, score);

    }

    public void ReadHighScore()
    {
        SaveData data = new SaveData();
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path)){
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<SaveData>(json);
        }
        prevScore = data.highScore;
        prevName = data.name;
    }

    public string GetPrevName()
    {
        return prevName;
    }

    public int GetPrevScore()
    {
        return prevScore;
    }

    public string GetThisName()
    {
        return thisName;
    }

    public void RecordNewScore(string name, int score)
    {
        SaveData data = new SaveData();
        data.name = name;
        data.highScore = score;

        string json = JsonUtility.ToJson(data);

        string path = Application.persistentDataPath + "/savefile.json";
        File.WriteAllText(path, json);

    }
   
}
