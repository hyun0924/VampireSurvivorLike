using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static WeaponController;

public class PlayerController : CreatureController
{
    Rigidbody2D _rb;
    PlayerStat _stat;
    UI_Hp _hpUI;

    Vector3 _moveVec = Vector3.zero;
    GameObject _bullets;
    bool _invincible = false;

    [SerializeField] protected int _killCount;
    public int KillCount
    {
        get { return _killCount; }
        set
        {
            _killCount = value;
            (Managers.UI.SceneUI as UI_Game).SetText(UI_Game.Texts.KillText, _killCount.ToString());
        }
    }

    [SerializeField] Define.State _state = Define.State.Idle;
    Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;

            switch (value)
            {
                case Define.State.Idle:
                    _anim.Play("WAIT");
                    break;
                case Define.State.Run:
                    _anim.Play("RUN");
                    break;
                case Define.State.Hit:
                    _sr.color = new Color32(255, 134, 134, 255);
                    break;
                case Define.State.Dead:
                    _anim.Play("DEAD");
                    break;
            }
        }
    }

    public void GetExp(int exp) { _stat.Exp += exp; }
    public void RecoverHp(int hp) { _stat.HP = Math.Min(_stat.HP + hp, _stat.MaxHp); }

    protected override void Init()
    {
        base.Init();

        _rb = GetComponent<Rigidbody2D>();
        _stat = gameObject.GetOrAddComponent<PlayerStat>();

        Managers.Input.NoInputAction += () => { if (State == Define.State.Run) State = Define.State.Idle; };

        Managers.Input.KeyBoardAction -= OnKeyBoard;
        Managers.Input.KeyBoardAction += OnKeyBoard;

        _bullets = Util.GetOrCreateGameObject("Bullets", transform);
        StartCoroutine("ShootBullet");
    }

    void Update()
    {
        Managers.TileMap.UpdateTileMap(transform.position, _moveVec);

        if (Input.GetKeyDown(KeyCode.Space))
            ShovelLevelUp();
    }

    IEnumerator ShootBullet()
    {
        WaitForSeconds sec = new WaitForSeconds(0.2f);
        while (true)
        {
            GameObject target = FindNearestEnemy();
            if (target == null)
            {
                yield return sec;
                continue;
            }
            yield return sec;
            Managers.Resource.Instantiate("Bullet 0").GetOrAddComponent<BulletController>().Init(transform, target);
        }
    }

    [SerializeField] private float _range = 5f;
    private GameObject FindNearestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _range, (1 << (int)Define.Layer.Enemy));

        if (colliders.Length == 0)
            return null;

        GameObject enemy = null;
        float minDistance = float.MaxValue;
        foreach (Collider2D collider in colliders)
        {
            float distance = (collider.transform.position - transform.position).sqrMagnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                enemy = collider.gameObject;
            }
        }

        return enemy;
    }

    int _shovelLevel = 1;
    private void ShovelLevelUp()
    {
        float degree = 360.0f / _shovelLevel;
        Managers.Resource.Instantiate("Shovel 0", _bullets.transform);

        for (int i = 0; i < _bullets.transform.childCount; i++)
            _bullets.transform.GetChild(i).GetOrAddComponent<ShovelController>().Init(degree * i);

        _shovelLevel++;
    }

    private void OnKeyBoard()
    {
        if (_state == Define.State.Dead) return;

        _moveVec = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            _moveVec += Vector3.up * _stat.Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _moveVec += Vector3.down * _stat.Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _moveVec += Vector3.left * _stat.Speed * Time.deltaTime;
            _sr.flipX = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _moveVec += Vector3.right * _stat.Speed * Time.deltaTime;
            _sr.flipX = false;
        }

        transform.position += _moveVec;
        if (_moveVec == Vector3.zero) State = Define.State.Idle;
        else State = Define.State.Run;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (_state == Define.State.Dead) return;

        if (other.gameObject.layer == (int)Define.Layer.Enemy && !_invincible)
        {
            int damage = other.gameObject.GetComponent<EnemyStat>().Damage;
            StartCoroutine(OnHit(damage));
        }
    }

    IEnumerator OnHit(int damage)
    {
        _invincible = true;
        State = Define.State.Hit;
        _stat.HP = Math.Max(0, _stat.HP - damage);

        if (_stat.HP <= 0)
        {
            _sr.color = _originColor;
            State = Define.State.Dead;
            _rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            yield break;
        }

        yield return new WaitForSeconds(_stat.HitDuration);
        _sr.color = _originColor;
        _invincible = false;
    }
}
