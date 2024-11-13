using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _speed = 3.0f;
    [SerializeField] int _level = 0;

    SpriteRenderer _sr;
    Animator _anim;

    Vector3 _moveVec = Vector3.zero;
    GameObject _bullets;
    float _hitDuration = 0.3f;
    bool _isAttacked = false;

    enum PlayerState
    {
        Idle,
        Run,
    }

    PlayerState _state = PlayerState.Idle;
    PlayerState State
    {
        get { return _state; }
        set
        {
            _state = value;

            switch (value)
            {
                case PlayerState.Idle:
                    _anim.Play("WAIT");
                    break;
                case PlayerState.Run:
                    _anim.Play("RUN");
                    break;
            }
        }
    }

    void Start()
    {
        Managers.Input.NoInputAction += () => { if (State != PlayerState.Idle) State = PlayerState.Idle; };

        Managers.Input.KeyBoardAction -= OnKeyBoard;
        Managers.Input.KeyBoardAction += OnKeyBoard;

        _bullets = Util.GetOrCreateGameObject("Bullets", transform);

        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    void UpdateIdle()
    {

    }

    void UpdateRun()
    {

    }

    void Update()
    {
        Managers.TileMap.UpdateTileMap(transform.position, _moveVec);

        switch (_state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Run:
                UpdateRun();
                break;
        }

        if (Input.GetKeyDown(KeyCode.Q))
            Managers.Resource.Instantiate("Bullet0").GetOrAddComponent<BulletController>().Init(transform);
    }

    private void LevelUp(int level)
    {
        float degree = 360.0f / level;
        for (int i = _level; i < level; i++)
            Managers.Resource.Instantiate("Shovel0", _bullets.transform);

        for (int i = 0; i < _bullets.transform.childCount; i++)
            _bullets.transform.GetChild(i).GetOrAddComponent<ShovelController>().Init(degree * i);

        _level = level;
    }

    private void OnKeyBoard()
    {
        _moveVec = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            _moveVec += Vector3.up * _speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _moveVec += Vector3.down * _speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _moveVec += Vector3.left * _speed * Time.deltaTime;
            _sr.flipX = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _moveVec += Vector3.right * _speed * Time.deltaTime;
            _sr.flipX = false;
        }

        transform.position += _moveVec;
        if (_moveVec == Vector3.zero) State = PlayerState.Idle;
        else State = PlayerState.Run;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)Define.Layer.Enemy && !_isAttacked)
        {
            StartCoroutine("HitAnimation");
        }
    }

    IEnumerator HitAnimation()
    {
        _isAttacked = true;
        Color originColor = _sr.color;
        _sr.color = new Color32(255, 134, 134, 255);
        yield return new WaitForSeconds(_hitDuration);
        _sr.color = originColor;
        yield return new WaitForSeconds(0.7f);
        _isAttacked = false;
    }
}
