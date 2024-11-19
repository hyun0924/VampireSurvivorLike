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
        StartCoroutine("UpdateTime");
    }

    IEnumerator UpdateTime()
    {
        Managers.Game.Second = 0;
        WaitForSeconds sec = new(1f);
        while (true)
        {
            SetText(Texts.TimeText, string.Format("{0:00}:{1:00}", Managers.Game.Second / 60, Managers.Game.Second % 60));
            Managers.Game.Second++;
            yield return sec;
        }
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
