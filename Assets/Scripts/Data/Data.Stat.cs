using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DataManager;

namespace Stat
{
    #region PlayerStat
    [Serializable]
    public class ExpStats
    {
        public int level;
        public int totalExp;
    }

    [Serializable]
    public class PlayerInfo
    {
        public int idx;
        public string name;
        public string desc;
        public string unlock;
        public List<int> stats;
        public List<int> color;
    }

    [Serializable]
    public class PlayerStatData : ILoader<int, ExpStats>, ILoader<int,  PlayerInfo>
    {
        public List<ExpStats> expStats = new List<ExpStats>();
        public List<PlayerInfo> playerInfos = new List<PlayerInfo>();

        public Dictionary<int, ExpStats> MakeDict()
        {
            Dictionary<int, ExpStats> dict = new Dictionary<int, ExpStats>();
            foreach (ExpStats expStat in expStats)
                dict.Add(expStat.level, expStat);
            return dict;
        }

        Dictionary<int, PlayerInfo> ILoader<int, PlayerInfo>.MakeDict()
        {
            Dictionary<int, PlayerInfo> dict = new Dictionary<int, PlayerInfo>();
            foreach (PlayerInfo playerInfo in playerInfos)
                dict.Add(playerInfo.idx, playerInfo);
            return dict;
        }
    }
    #endregion

    #region EnemyStat
    [Serializable]
    public class EnemyStat
    {
        public string name;
        public int maxHp;
        public int damage;
        public int speed;
    }

    [Serializable]
    public class EnemyStatData : ILoader<string, EnemyStat>
    {
        public List<EnemyStat> enemyStats = new List<EnemyStat>();
        public Dictionary<string, EnemyStat> MakeDict()
        {
            Dictionary<string, EnemyStat> dict = new Dictionary<string, EnemyStat>();
            foreach (EnemyStat enemyStat in enemyStats)
                dict.Add(enemyStat.name, enemyStat);
            return dict;
        }
    }
    #endregion

    #region Selections
    [Serializable]
    public class desc
    {
        public int level;
        public string description;
    }

    [Serializable]
    public class weaponSelection
    {
        public string name;
        public string imageName;
        public List<desc> descriptions;

        public string GetDesc(int level)
        {
            return descriptions[level].description;
        }
    }

    [Serializable]
    public class statSelection
    {
        public string name;
        public string imageName;
        public string description;
    }

    [Serializable]
    public class SelectionData: ILoader<string, weaponSelection>, ILoader<string, statSelection>
    {
        public List<weaponSelection> weaponSelections;
        public List<statSelection> statSelections;

        public Dictionary<string, weaponSelection> MakeDict()
        {
            Dictionary<string, weaponSelection> dict = new Dictionary<string, weaponSelection>();
            foreach (weaponSelection s in weaponSelections)
            {
                dict.Add(s.name, s);
            }
            return dict;
        }

        Dictionary<string, statSelection> ILoader<string, statSelection>.MakeDict()
        {
            Dictionary<string, statSelection> dict = new Dictionary<string, statSelection>();
            foreach (statSelection s in statSelections)
            {
                dict.Add(s.name, s);
            }
            return dict;
        }
    }
    #endregion
}