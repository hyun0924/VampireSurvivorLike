using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Game : UI_Scene
{
    public enum Texts
    {
        LevelText,
        KillText,
        TimeText,
    }

    enum GameObjects
    {
        ExpSlider,
    }


    protected override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));

        Get<TextMeshProUGUI>((int)Texts.KillText).text = "0";
    }

    public void SetExpSlider(float value)
    {
        GetGameObject((int)GameObjects.ExpSlider).GetComponent<Slider>().value = value;
    }

    public void SetText(Texts type, string text)
    {
        GetText((int)type).text = text;
    }
}
