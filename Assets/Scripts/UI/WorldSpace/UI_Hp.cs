using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Hp : UI_Base
{
    enum GameObjects
    {
        HpSlider,
    }

    BaseStat _stat;
    Slider _hpSlider;

    protected override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        _stat = transform.parent.GetComponent<BaseStat>();
        _hpSlider = Get<GameObject>((int)GameObjects.HpSlider).GetComponent<Slider>();
    }

    private void Update()
    {
        transform.position = transform.parent.position + Vector3.down * transform.parent.GetComponent<BoxCollider2D>().size.y * 0.8f;

        SetHPRatio();
    }

    private void SetHPRatio()
    {
        _hpSlider.value = (float)_stat.HP / _stat.MaxHp;
    }
}
