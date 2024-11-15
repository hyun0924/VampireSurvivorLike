using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PropController : Poolable
{
    GameObject _player;
    Animator _anim;
    float _speed = 0;

    enum States
    {
        Idle,
        Drag,
    }
    States _state = States.Idle;
    States State { get { return _state; } set { _state = value; _anim.enabled = _state == States.Idle; } }

    Define.Prop _type;

    public void Init(Vector3 position, Define.Prop type)
    {
        transform.position = position;
        _type = type;
        gameObject.GetOrAddComponent<Poolable>();
    }

    private void Start()
    {
        if (_type == Define.Prop.Exp) _anim = transform.GetChild(0).GetComponent<Animator>();
        else _anim = GetComponent<Animator>();
        _player = Managers.Game.Player.gameObject;
    }

    private void Update()
    {
        if (State == States.Drag)
        {
            Vector3 dest = _player.transform.position;
            Vector3 dir = dest - transform.position;

            _speed += 8 * Time.deltaTime;

            transform.position += dir.normalized * _speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != (int)Define.Layer.Player) return;

        switch(_type)
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
                    other.gameObject.GetComponent<PlayerController>().GetExp(exp);
                }
                break;
            case Define.Prop.Magnet:
                {
                    Debug.Log("Magnet");
                    for (int i = 0; i < Managers.Game.Exps.transform.childCount; i++)
                    {
                        Managers.Game.Exps.transform.GetChild(i).GetComponent<PropController>().State = States.Drag;
                    }
                }
                break;
            case Define.Prop.Heal:
                {
                    Debug.Log("Heal");
                }
                break;
        }

        Managers.Resource.Destroy(gameObject);
    }
}