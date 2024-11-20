using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameClear : UI_Popup
{
    enum Texts
    {
        KillText,
        TimeText
    }

    enum Buttons
    {
        ReturnButton,
    }

    protected override void Init()
    {
        base.Init();

        Managers.Sound.Play(Define.Audio.Win);

        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        Time.timeScale = 0;
        if (Managers.Game.IsLocked[2])
            Managers.Game.IsLocked[2] = false;

        GetText((int)Texts.KillText).text = Managers.Game.Player.StatPlayer.KillCount.ToString();
        GetText((int)Texts.TimeText).text = string.Format("{0:00}:{1:00}", Managers.Game.GameSecond / 60, Managers.Game.GameSecond % 60);

        Button button = GetButton((int)Buttons.ReturnButton);
        BindEvent(button.gameObject, (PointerEventData) =>
        {
            Managers.Scene.LoadScene(Define.Scene.Login);
            Time.timeScale = 1;
            Managers.UI.ClosePopupUI();
        });
    }
}
