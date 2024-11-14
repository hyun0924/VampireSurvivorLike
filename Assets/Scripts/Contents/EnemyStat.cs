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
        if (!Enum.TryParse(name, out _type))
        {
            Debug.Log($"Incorrect name: {name}");
            _type = Define.EnemyType.Enemy_0;
        }
        _speed = 3f;
        _maxHp = 20;
        _hp = 20;
        _damage = 2;
        _detectDistance = 20f;
        _hitDuration = 0.15f;
        _deadDuration = 1f;
    }
}