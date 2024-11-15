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
    public class PlayerStatData : ILoader<int, ExpStats>
    {
        public List<ExpStats> expStats = new List<ExpStats>();

        public Dictionary<int, ExpStats> MakeDict()
        {
            Dictionary<int, ExpStats> dict = new Dictionary<int, ExpStats>();
            foreach (ExpStats expStat in expStats)
                dict.Add(expStat.level, expStat);
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
}