using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    protected SpriteRenderer _sr;
    protected Animator _anim;
    protected Color _originColor;
    protected UI_Hp _hpUI;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        _sr = GetComponent<SpriteRenderer>();
        _originColor = _sr.color;

        _anim = GetComponent<Animator>();

        _hpUI = Managers.UI.AddWorldSpaceUI<UI_Hp>(transform);
    }
}
