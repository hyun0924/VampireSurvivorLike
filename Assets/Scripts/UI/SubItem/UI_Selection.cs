using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Selection : UI_Base
{
    enum Images
    {
        IconImage,
    }

    enum Texts
    {
        NameText,
        DescText,
    }

    protected override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        BindEvent(gameObject, OnClick);
    }

    private void OnClick(PointerEventData data)
    {
        Managers.Resource.Destroy(transform.parent.parent.gameObject);
        Time.timeScale = 1.0f;
    }
}
