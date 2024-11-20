using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Login : UI_Scene
{
    enum Buttons
    {
        StartButton,
    }
    protected override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Button button = GetButton((int)Buttons.StartButton);
        BindEvent(button.gameObject, (PointerEventData) =>
        {
            Managers.Sound.Play(Define.Audio.Select);
            Managers.UI.ShowPopupUI<UI_CharacterSelect>();
        });
    }
}
