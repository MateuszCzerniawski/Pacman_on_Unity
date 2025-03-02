using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class HighScoreRecord
{
    public string name="";
    public int score=0;
}
[Serializable]
public class HighScoreList
{
    public static readonly string HighScoreKey = "HighScores";
    [SerializeField] private List<HighScoreRecord> _records = new List<HighScoreRecord>();
    public static HighScoreList Load()
    {
        return PlayerPrefs.HasKey(HighScoreKey)?JsonUtility.FromJson<HighScoreList>(PlayerPrefs.GetString(HighScoreKey)):new HighScoreList();
    }

    public void SaveHighScores()
    {
        PlayerPrefs.SetString(HighScoreKey,JsonUtility.ToJson(this));
        PlayerPrefs.Save();
    }

    public static void Reset()
    {
        PlayerPrefs.DeleteKey(HighScoreKey);
    }

    public void AddRecord(string name,int score)
    {
        HighScoreRecord record = new HighScoreRecord() { name = name, score = score };
        _records.Add(record);
        _records = _records.OrderByDescending(e => e.score).ToList();
    }

    public List<HighScoreRecord> GetHighScores(int top)
    {
        return top > 0 ? _records.Take(top).ToList() : new List<HighScoreRecord>();
    }

    public List<HighScoreRecord> GetHighScores()
    {
        return _records.Select(e=>new HighScoreRecord(){name=e.name,score = e.score}).ToList();
    }
}
