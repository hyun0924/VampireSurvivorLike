using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public Dictionary<int, Stat.ExpStats> ExpStatsDict { get; private set; } = new Dictionary<int, Stat.ExpStats>();
    public Dictionary<int, Stat.PlayerInfo> PlayerInfoDict { get; private set; } = new Dictionary<int, Stat.PlayerInfo>();
    public Dictionary<string, Stat.EnemyStat> EnemyStatDict { get; private set; } = new Dictionary<string, Stat.EnemyStat>();
    public Dictionary<string, Stat.weaponSelection> WeaponSelectionDict { get; private set; }
    public Dictionary<string, Stat.statSelection> StatSelectionDict { get; private set; }

    public interface ILoader<Key, Value>
    {
        public Dictionary<Key, Value> MakeDict();
    }


    public void Init()
    {
        ExpStatsDict = Load<Stat.PlayerStatData, int, Stat.ExpStats>("PlayerStatData");
        PlayerInfoDict = Load<Stat.PlayerStatData, int, Stat.PlayerInfo>("PlayerStatData");
        EnemyStatDict = Load<Stat.EnemyStatData, string, Stat.EnemyStat>("EnemyStatData");
        WeaponSelectionDict = Load<Stat.SelectionData, string, Stat.weaponSelection>("SelectionData");
        StatSelectionDict = Load<Stat.SelectionData, string, Stat.statSelection>("SelectionData");
    }

    public Dictionary<Key, Value> Load<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        if (!path.Contains("Data/"))
            path = "Data/" + path;

        TextAsset textAsset = Managers.Resource.Load<TextAsset>(path);
        return JsonUtility.FromJson<Loader>(textAsset.text).MakeDict();
    }
}