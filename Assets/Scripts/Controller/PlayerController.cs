using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static WeaponController;

public class PlayerController : CreatureController
{
    Rigidbody2D _rb;
    public PlayerStat StatPlayer { get { return gameObject.GetOrAddComponent<PlayerStat>(); } }
    public int[] Levels = new int[(int)Define.PlayerLevel.MaxCount];

    GameObject _bullets;
    SpriteRenderer _leftHand;
    SpriteRenderer _rightHand;
    Sprite _leftHandSprite;
    Sprite _rightHandSprite;
    Sprite _shovelSprite;
    Sprite _bulletSprite;

    Vector3 _moveVec = Vector3.zero;
    bool _invincible = false;
    Sprite[] _sprites;

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
                    {
                        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("RIGHT_RUN")) _anim.Play("RIGHT_WAIT");
                        else _anim.Play("LEFT_WAIT");
                    }
                    break;
                case Define.State.Run:
                    {
                        if (_moveVec.x > 0) _anim.Play("RIGHT_RUN");
                        else if (_moveVec.x < 0) _anim.Play("LEFT_RUN");
                        else
                        {
                            int name = _anim.GetCurrentAnimatorStateInfo(0).shortNameHash;
                            int a = Animator.StringToHash("RIGHT_WAIT");
                            int b = Animator.StringToHash("LEFT_WAIT");

                            if (_anim.GetCurrentAnimatorStateInfo(0).IsName("RIGHT_WAIT")
                                || _anim.GetCurrentAnimatorStateInfo(0).IsName("RIGHT_RUN")) _anim.Play("RIGHT_RUN");
                            else _anim.Play("LEFT_RUN");
                        }
                    }
                    break;
                case Define.State.Hit:
                    _sr.color = new Color32(255, 134, 134, 255);
                    break;
                case Define.State.Dead:
                    {
                        _anim.Play("DEAD");
                        StopCoroutine(Enum.GetName(typeof(Define.Gun), Managers.Game.CurrentPlayerType));

                        for (int i = 0; i < _bullets.transform.childCount; i++)
                            Managers.Resource.Destroy(_bullets.transform.GetChild(i).gameObject);

                        Managers.UI.ShowPopupUI<UI_GameOver>();
                    }
                    break;
            }
        }
    }

    protected override void Init()
    {
        _rb = GetComponent<Rigidbody2D>();

        base.Init();

        Managers.Input.NoInputAction -= () => { if (State == Define.State.Run) State = Define.State.Idle; };
        Managers.Input.NoInputAction += () => { if (State == Define.State.Run) State = Define.State.Idle; };

        Managers.Input.KeyBoardAction -= OnKeyBoard;
        Managers.Input.KeyBoardAction += OnKeyBoard;

        _bullets = Util.GetOrCreateGameObject("Bullets", transform);
        LoadSprites();

        Stat.PlayerInfo info;
        if (!Managers.Data.PlayerInfoDict.TryGetValue((int)Managers.Game.CurrentPlayerType, out info))
            Debug.Log("No Player Data");

        for (int i = 0; i < (int)Define.PlayerLevel.MaxCount; i++)
        {
            Levels[i] = 0;

            while (Levels[i] < info.stats[i])
                LevelUp((Define.PlayerLevel)i);
        }
    }

    private void LoadSprites()
    {
        _sprites = Resources.LoadAll<Sprite>($"Arts/Sprites/Farmer_{(int)Managers.Game.CurrentPlayerType}");
        _leftHand = gameObject.FindChild<SpriteRenderer>("LeftHand");
        _rightHand = gameObject.FindChild<SpriteRenderer>("RightHand");
        _leftHandSprite = Managers.Resource.LoadSubSprite("Props", $"Weapon {(int)Managers.Game.CurrentPlayerType}");
        _rightHandSprite = Managers.Resource.LoadSubSprite("Props", $"Weapon {(int)Define.Player.MaxCount + (int)Managers.Game.CurrentPlayerType}");
        _shovelSprite = Managers.Resource.LoadSubSprite("Props", $"Bullet {(int)Managers.Game.CurrentPlayerType}");
        _bulletSprite = Managers.Resource.LoadSubSprite("Props", $"Bullet {(int)Define.Player.MaxCount + (int)Managers.Game.CurrentPlayerType}");
    }

    void Update()
    {
        if (State == Define.State.Dead || Managers.Game.IsClear) return;

        Managers.TileMap.UpdateTileMap(transform.position, _moveVec);

        if (Input.GetKeyDown(KeyCode.Space))
            LevelUp(Define.PlayerLevel.Gun);
        if (Input.GetKeyDown(KeyCode.Q))
            LevelUp(Define.PlayerLevel.Shovel);
    }

    #region 총 종류별 구현
    IEnumerator Riple()
    {
        WaitForSeconds sec = new WaitForSeconds(0.2f);
        while (true)
        {
            GameObject target = gameObject.FindNearestEnemy(8f);
            if (target == null)
            {
                yield return sec;
                continue;
            }

            for (int i = 0; i < Levels[(int)Define.PlayerLevel.Gun] + 1; i++)
            {
                Managers.Resource.Instantiate("Weapons/Bullet").GetOrAddComponent<BulletController>().Init(transform, target.transform.position, _bulletSprite, 4);
                yield return sec;
            }
            yield return new WaitForSeconds(0.5f / StatPlayer.Cooldown);
        }
    }

    IEnumerator Shotgun()
    {
        WaitForSeconds sec = new WaitForSeconds(0.2f);
        while (true)
        {
            GameObject target = gameObject.FindNearestEnemy(5f);
            if (target == null)
            {
                yield return sec;
                continue;
            }

            Vector3 dir = target.transform.position - transform.position;

            float degree;
            if (Levels[(int)Define.PlayerLevel.Gun] % 2 == 1)
            {
                degree = 7.5f;
            }
            else
            {
                degree = 15f;
                Managers.Resource.Instantiate("Weapons/Bullet").GetOrAddComponent<BulletController>().Init(transform, target.transform.position, _bulletSprite, 5);
            }

            int a = (Levels[(int)Define.PlayerLevel.Gun] + 1) / 2;
            for (int i = 0; i < a; i++)
            {
                float tan = MathF.Tan((degree + 15f * i) * Mathf.Deg2Rad);
                Vector3 v1 = transform.position + new Vector3(dir.x + dir.y * tan, dir.y - dir.x * tan);
                Vector3 v2 = transform.position + new Vector3(dir.x - dir.y * tan, dir.y + dir.x * tan);

                Managers.Resource.Instantiate("Weapons/Bullet").GetOrAddComponent<BulletController>().Init(transform, v1, _bulletSprite, 5);
                Managers.Resource.Instantiate("Weapons/Bullet").GetOrAddComponent<BulletController>().Init(transform, v2, _bulletSprite, 5);
            }

            yield return new WaitForSeconds(1.5f / StatPlayer.Cooldown);
        }
    }

    IEnumerator Sniper()
    {
        WaitForSeconds sec = new WaitForSeconds(0.2f);
        while (true)
        {
            GameObject target = gameObject.FindNearestEnemy(12f);
            if (target == null)
            {
                yield return sec;
                continue;
            }

            GameObject go = Managers.Resource.Instantiate("Weapons/Bullet");
            go.GetOrAddComponent<BulletController>().Init(transform, target.transform.position, _bulletSprite, 10, Levels[(int)Define.PlayerLevel.Gun], 12f);
            yield return new WaitForSeconds(2.0f / StatPlayer.Cooldown);
        }
    }
    #endregion

    public void LevelUp(Define.PlayerLevel type)
    {
        Levels[(int)type]++;
        int level = Levels[(int)type];
        switch (type)
        {
            case Define.PlayerLevel.Shovel:
                {
                    if (level == 1)
                        _leftHand.sprite = _leftHandSprite;

                    float degree = 360.0f / level;
                    GameObject shovel = Managers.Resource.Instantiate($"Weapons/Shovel", _bullets.transform);
                    shovel.GetComponent<SpriteRenderer>().sprite = _shovelSprite;

                    for (int i = 0; i < _bullets.transform.childCount; i++)
                        _bullets.transform.GetChild(i).GetOrAddComponent<ShovelController>().Init(degree * i);
                }
                break;
            case Define.PlayerLevel.Gun:
                {
                    if (level == 1)
                    {
                        _rightHand.sprite = _rightHandSprite;
                        StartCoroutine(Enum.GetName(typeof(Define.Gun), Managers.Game.CurrentPlayerType));
                    }
                }
                break;
            case Define.PlayerLevel.Speed:
                {
                    StatPlayer.Speed *= 1.03f;
                }
                break;
            case Define.PlayerLevel.MaxHP:
                {
                    StatPlayer.MaxHp = StatPlayer.MaxHp + 5;
                    _hpUI.SetSliderWidth();
                }
                break;
            case Define.PlayerLevel.Power:
                {
                    StatPlayer.Damage = (int)(StatPlayer.Damage * 1.07f);
                }
                break;
            case Define.PlayerLevel.Cooldown:
                {
                    StatPlayer.Cooldown *= 1.1f;
                }
                break;
        }
    }

    private void OnKeyBoard()
    {
        if (_state == Define.State.Dead) return;

        _moveVec = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            _moveVec += Vector3.up * StatPlayer.Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _moveVec += Vector3.down * StatPlayer.Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _moveVec += Vector3.left * StatPlayer.Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _moveVec += Vector3.right * StatPlayer.Speed * Time.deltaTime;
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
            Managers.Sound.Play(Define.Audio.Melee1);
            int damage = other.gameObject.GetComponent<EnemyStat>().Damage;
            StartCoroutine(OnHit(damage));
        }
    }

    IEnumerator OnHit(int damage)
    {
        _invincible = true;
        State = Define.State.Hit;
        StatPlayer.HP = Math.Max(0, StatPlayer.HP - damage);

        if (StatPlayer.HP <= 0)
        {
            _sr.color = _originColor;
            State = Define.State.Dead;
            _rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            yield break;
        }

        yield return new WaitForSeconds(StatPlayer.HitDuration);
        _sr.color = _originColor;
        _invincible = false;
    }

    public void SetSprite(Define.PlayerSprite spriteName)
    {
        _sr.sprite = _sprites[(int)spriteName];
    }
}
