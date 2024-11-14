using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Upgrade : UI_Popup
{
    enum GameObjects
    {
        Selections,
    }

    GameObject _selections = null;

    protected override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        _selections = GetGameObject((int)GameObjects.Selections);

        for (int i = 0; i < 3; i++)
        {
            Managers.UI.AddSubItemUI<UI_Selection>(_selections.transform);
        }
    }
}
