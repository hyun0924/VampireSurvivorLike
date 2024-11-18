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
    public PlayerStat Stat { get { return _stat; } }

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
                    if (_anim.GetCurrentAnimatorStateInfo(0).IsName("RIGHT_RUN")) _anim.Play("RIGHT_WAIT");
                    else _anim.Play("LEFT_WAIT");
                    break;
                case Define.State.Run:
                    if (_moveVec.x > 0) _anim.Play("RIGHT_RUN");
                    else _anim.Play("LEFT_RUN");
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
        _stat = gameObject.GetOrAddComponent<PlayerStat>();
        base.Init();

        _rb = GetComponent<Rigidbody2D>();

        Managers.Input.NoInputAction += () => { if (State == Define.State.Run) State = Define.State.Idle; };

        Managers.Input.KeyBoardAction -= OnKeyBoard;
        Managers.Input.KeyBoardAction += OnKeyBoard;

        _bullets = Util.GetOrCreateGameObject("Bullets", transform);
        StartCoroutine("Shotgun");
    }

    void Update()
    {
        Managers.TileMap.UpdateTileMap(transform.position, _moveVec);

        if (Input.GetKeyDown(KeyCode.Space))
            ShovelLevelUp();
    }

    private int _ripleLevel = 0;
    public int RipleLevel
    {
        get { return _ripleLevel; }
        set { _ripleLevel = value; }
    }

    IEnumerator Riple()
    {
        WaitForSeconds sec = new WaitForSeconds(0.2f);
        while (true)
        {
            GameObject target = FindNearestEnemy(8f);
            if (target == null)
            {
                yield return sec;
                continue;
            }
            yield return new WaitForSeconds(0.5f / Mathf.Pow(1.1f, _stat.Cooldown));

            for (int i = 0; i < _ripleLevel + 2; i++)
            {
                Managers.Resource.Instantiate("Bullet 0").GetOrAddComponent<BulletController>().Init(transform, target.transform.position, _stat.Damage);
                yield return sec;
            }
        }
    }


    private int _shotgunLevel = 0;
    public int ShotgunLevel
    {
        get { return _shotgunLevel; }
        set { _shotgunLevel = value; }
    }

    IEnumerator Shotgun()
    {
        WaitForSeconds sec = new WaitForSeconds(0.2f);
        while (true)
        {
            GameObject target = FindNearestEnemy(5f);
            if (target == null)
            {
                yield return sec;
                continue;
            }
            yield return new WaitForSeconds(1.0f / Mathf.Pow(1.1f, _stat.Cooldown));

            Vector3 dir = target.transform.position - transform.position;

            if (_shotgunLevel % 2 == 0)
            {
                int a = (_shotgunLevel + 2) / 2;
                for (int i = 0; i < a; i++)
                {
                    float tan = MathF.Tan((7.5f + 15f * i) * Mathf.Deg2Rad);
                    Vector3 v1 = transform.position + new Vector3(dir.x + dir.y * tan, dir.y - dir.x * tan);
                    Vector3 v2 = transform.position + new Vector3(dir.x - dir.y * tan, dir.y + dir.x * tan);

                    Managers.Resource.Instantiate("Bullet 0").GetOrAddComponent<BulletController>().Init(transform, v1, _stat.Damage);
                    Managers.Resource.Instantiate("Bullet 0").GetOrAddComponent<BulletController>().Init(transform, v2, _stat.Damage);
                }
            }
            else
            {
                int a = (_shotgunLevel + 2) / 2;
                Managers.Resource.Instantiate("Bullet 0").GetOrAddComponent<BulletController>().Init(transform, target.transform.position, _stat.Damage);

                for (int i = 1; i <= a; i++)
                {
                    float tan = MathF.Tan(15f * i * Mathf.Deg2Rad);
                    Vector3 v1 = transform.position + new Vector3(dir.x + dir.y * tan, dir.y - dir.x * tan);
                    Vector3 v2 = transform.position + new Vector3(dir.x - dir.y * tan, dir.y + dir.x * tan);

                    Managers.Resource.Instantiate("Bullet 0").GetOrAddComponent<BulletController>().Init(transform, v1, _stat.Damage);
                    Managers.Resource.Instantiate("Bullet 0").GetOrAddComponent<BulletController>().Init(transform, v2, _stat.Damage);
                }
            }
        }
    }

    private GameObject FindNearestEnemy(float range)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range, (1 << (int)Define.Layer.Enemy));

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
    public void ShovelLevelUp()
    {
        float degree = 360.0f / _shovelLevel;
        Managers.Resource.Instantiate("Shovel 0", _bullets.transform);

        for (int i = 0; i < _bullets.transform.childCount; i++)
            _bullets.transform.GetChild(i).GetOrAddComponent<ShovelController>().Init(degree * i);

        _shovelLevel++;
    }

    int _speedLevel = 0;
    int _hpLevel = 0;
    int _powerLevel = 0;
    public void StatLevelUp(Define.StatType type)
    {
        switch (type)
        {
            case Define.StatType.Speed:
                _speedLevel++;
                _stat.Speed *= 1.03f;
                break;
            case Define.StatType.MaxHP:
                _hpLevel++;
                _stat.MaxHp = _stat.MaxHp + 5;
                _hpUI.SetSliderWidth();
                break;
            case Define.StatType.Power:
                _powerLevel++;
                _stat.Damage = (int)(_stat.Damage * 1.07f);
                break;
            case Define.StatType.Cooldown:
                _stat.Cooldown++;
                break;
        }
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
        }
        if (Input.GetKey(KeyCode.D))
        {
            _moveVec += Vector3.right * _stat.Speed * Time.deltaTime;
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
