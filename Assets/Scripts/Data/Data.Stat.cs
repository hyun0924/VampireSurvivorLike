using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DataManager;

namespace Stat
{
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
}