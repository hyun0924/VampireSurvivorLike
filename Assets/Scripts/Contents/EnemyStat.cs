using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : BaseStat
{
    [SerializeField] Define.EnemyType _type;
    [SerializeField] float _detectDistance;
    [SerializeField] float _deadDuration;

    public Define.EnemyType Type { get { return _type; } }
    public float DetectDistance {  get { return _detectDistance; } set { _detectDistance = value; } }
    public float DeadDuration { get { return _deadDuration; } set { _deadDuration = value; } }

    private void Start()
    {
        Stat.EnemyStat enemyStat = null;
        if (Managers.Data.EnemyStatDict.ContainsKey(name))
            enemyStat = Managers.Data.EnemyStatDict[name];
        else
            enemyStat = Managers.Data.EnemyStatDict["Enemy_0"];

        _type = GetType(enemyStat.name);
        _speed = enemyStat.speed;
        _maxHp = enemyStat.maxHp;
        _hp = _maxHp;
        _damage = enemyStat.damage;
        _detectDistance = 15f;
        _hitDuration = 0.15f;
        _deadDuration = 1f;
    }

    private Define.EnemyType GetType(string name)
    {
        Define.EnemyType type = Define.EnemyType.Enemy_0;
        Enum.TryParse(name, out type);
        return type;
    }
}