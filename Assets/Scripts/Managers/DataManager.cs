using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public Dictionary<int, Stat.ExpStats> ExpStatsDict { get; private set; } = new Dictionary<int, Stat.ExpStats>();

    public interface ILoader<Key, Value>
    {
        public Dictionary<Key, Value> MakeDict();
    }


    public void Init()
    {
        ExpStatsDict = Load<Stat.PlayerStatData, int, Stat.ExpStats>("PlayerStatData");
    }

    public Dictionary<Key, Value> Load<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        if (!path.Contains("Data/"))
            path = "Data/" + path;

        TextAsset textAsset = Managers.Resource.Load<TextAsset>(path);
        return JsonUtility.FromJson<Loader>(textAsset.text).MakeDict();
    }
}