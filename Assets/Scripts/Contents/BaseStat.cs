using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStat : MonoBehaviour
{
    [SerializeField] protected float _speed;
    [SerializeField] protected int _maxHp;
    [SerializeField] protected int _hp;
    [SerializeField] protected int _damage;
    [SerializeField] protected float _hitDuration;

    public float Speed { get { return _speed; } set { _speed = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int HP { get { return _hp; } set { _hp = value; } }
    public int Damage { get { return _damage; } set { _damage = value; } }
    public float HitDuration { get { return _hitDuration; } set { _hitDuration = value; } }

    private void Start()
    {
        _speed = 3f;
        _maxHp = 20;
        _hp = 20;
        _hitDuration = 0.15f;
    }
}
