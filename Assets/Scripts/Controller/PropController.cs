using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class PropController : Poolable
{
    GameObject _player;
    Animator _anim;
    float _dragSpeed = 0;
    int _attackedCount = 0;
    bool _isShown = false;

    enum States
    {
        Idle,
        Drag,
    }
    States _state;
    States State { get { return _state; } set { _state = value; _anim.enabled = _state == States.Idle; } }

    [SerializeField] Define.Prop _type;

    public void Init(Vector3 position, Define.Prop type)
    {
        _type = type;
        _state = States.Idle;

        Respawn(position);
    }

    public override void Respawn(Vector3 position)
    {
        transform.position = position;
        _dragSpeed = 0;
        _attackedCount = 0;
        _isShown = false;

        if (_type == Define.Prop.Box)
            StartCoroutine("ReserveDestroy");
    }

    IEnumerator ReserveDestroy()
    {
        yield return new WaitForSeconds(20f);
        if (!_isShown)
            Managers.Resource.Destroy(gameObject);
    }

    private void Start()
    {
        if (_type != Define.Prop.Box) _anim = transform.GetChild(0).GetComponent<Animator>();
        _player = Managers.Game.Player.gameObject;
    }

    private void Update()
    {
        if (State == States.Drag)
        {
            Vector3 dest = _player.transform.position;
            Vector3 dir = dest - transform.position;

            _dragSpeed += 8 * Time.deltaTime;

            transform.position += dir.normalized * _dragSpeed * Time.deltaTime;
        }

        if (!_isShown && _type == Define.Prop.Box)
        {
            Vector3 viewport = Camera.main.WorldToViewportPoint(transform.position);
            if (0 <= viewport.x && viewport.x <= 1 && 0 <= viewport.y && viewport.y <= 1)
            {
                _isShown = true;
                transform.parent = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_type == Define.Prop.Box)
        {
            if ((other.gameObject.layer == (int)Define.Layer.Bullet || other.gameObject.layer == (int)Define.Layer.Shovel) && _attackedCount < 2)
            {
                _attackedCount++;
                if (_attackedCount == 2)
                {
                    CreateRandomProp();
                    Managers.Resource.Destroy(gameObject);
                }
            }
        }
        else if (other.gameObject.layer == (int)Define.Layer.Player)
        {
            switch (_type)
            {
                case Define.Prop.Exp:
                    {
                        int exp = 0;
                        switch (gameObject.name)
                        {
                            case "Exp 0":
                                exp = 1;
                                break;
                            case "Exp 1":
                                exp = 5;
                                break;
                            case "Exp 2":
                                exp = 10;
                                break;
                        }
                        other.gameObject.GetComponent<PlayerController>().Stat.Exp += exp;
                    }
                    break;
                case Define.Prop.Magnet:
                    {
                        for (int i = 0; i < Managers.Game.Props.transform.childCount; i++)
                        {
                            PropController controller = Managers.Game.Props.transform.GetChild(i).GetComponent<PropController>();
                            if (controller._type == Define.Prop.Exp)
                                controller.State = States.Drag;
                        }
                    }
                    break;
                case Define.Prop.Heal:
                    {
                        other.gameObject.GetComponent<PlayerController>().Stat.HP += 10; // TODO: 회복량 하드 코딩되어있음
                    }
                    break;
            }

            Managers.Resource.Destroy(gameObject);
        }
    }

    private void CreateRandomProp()
    {
        int idx = Random.Range((int)Define.Prop.Box + 1, (int)Define.Prop.BoxEnd);
        string name = Enum.GetName(typeof(Define.Prop), idx);

        GameObject prop = Managers.Resource.Instantiate(name, Managers.Game.Props.transform);
        prop.GetOrAddComponent<PropController>().Init(transform.position, (Define.Prop)idx);
    }
}