using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemyController : CreatureController, IPoolable
{
    BoxCollider2D _collider;
    EnemyStat _stat;

    GameObject _target = null;
    Sprite _hitSprite = null;
    Sprite _deadSprite = null;

    Define.State _state = Define.State.Run;
    Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;

            switch (value)
            {
                case Define.State.Run:
                    {
                        _anim.enabled = true;
                        gameObject.layer = (int)Define.Layer.Enemy;
                        _anim.Play("RUN");
                        _sr.color = _originColor;
                        _sr.sortingOrder = 2;
                        _collider.enabled = true;
                    }
                    break;
                case Define.State.Hit:
                    {
                        _anim.enabled = false;
                        _sr.sprite = _hitSprite;
                        _sr.color = new Color32(255, 134, 134, 255);
                        Vector3 dir = transform.position - _target.transform.position;
                        transform.position += dir.normalized * 0.5f;
                    }
                    break;
                case Define.State.Dead:
                    {
                        _anim.enabled = false;
                        gameObject.layer = (int)Define.Layer.Dead;
                        _sr.sprite = _deadSprite;
                        _sr.color = _originColor;
                        _sr.sortingOrder = 1;
                        _collider.enabled = false;
                    }
                    break;
            }
        }
    }

    protected override void Init()
    {
        base.Init();

        _collider = GetComponent<BoxCollider2D>();
        _stat = gameObject.GetOrAddComponent<EnemyStat>();

        string imageName = Enum.GetName(typeof(Define.EnemyType), _stat.Type);
        _hitSprite = Managers.Resource.LoadSubSprite(imageName, "Hit");
        _deadSprite = Managers.Resource.LoadSubSprite(imageName, "Dead");

        _target = Managers.Game.Player.gameObject;
    }

    public void Respawn(Vector3 position)
    {
        transform.position = position;
        State = Define.State.Run;
        _stat.HP = _stat.MaxHp;
    }

    private void Update()
    {
        if (State != Define.State.Run) return;

        if (_target == null)
            _target = Physics2D.OverlapCircle(transform.position, _stat.DetectDistance, (1 << (int)Define.Layer.Player))?.gameObject;
        else
        {
            Vector3 dest = _target.transform.position;
            Vector3 dir = dest - transform.position;

            if (dir.magnitude > _stat.DetectDistance)
            {
                float degree = Random.Range(0, 360.0f);
                transform.position = _target.transform.position + new Vector3(Mathf.Cos(degree * Mathf.Deg2Rad), Mathf.Sin(degree * Mathf.Deg2Rad)) * 10f;
            }

            _sr.flipX = (dir.x < 0);
            transform.position += dir.normalized * _stat.Speed * Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)Define.Layer.Shovel && State != Define.State.Hit)
        {
            ShovelController controller = collision.GetComponent<ShovelController>();
            StartCoroutine(OnHit(controller.GetDamage()));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)Define.Layer.Bullet)
        {
            BulletController controller = collision.GetComponent<BulletController>();
            StartCoroutine(OnHit(controller.GetDamage()));
        }
    }

    IEnumerator OnHit(int damage)
    {
        if (State == Define.State.Dead) yield break;

        State = Define.State.Hit;
        _stat.HP = Math.Max(0, _stat.HP - damage);

        if (_stat.HP <= 0)
        {
            State = Define.State.Dead;
            Managers.Game.Player.Stat.KillCount++;
            yield return new WaitForSeconds(_stat.DeadDuration);

            GameObject exp = Managers.Resource.Instantiate("Exp 0", Managers.Game.Props.transform);
            exp.GetOrAddComponent<PropController>().Init(transform.position, Define.Prop.Exp);
            
            Managers.Resource.Destroy(gameObject);
            yield break;
        }

        yield return new WaitForSeconds(_stat.HitDuration);

        if (State == Define.State.Dead) yield break;
        State = Define.State.Run;
    }
}
