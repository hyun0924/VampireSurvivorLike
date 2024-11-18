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
        int range = (int)Define.WeaponType.MaxCount + (int)Define.StatType.MaxCount; // TODO: weapon은 player type 별로 다르게 해야함, 중복 선택?

        for (int i = 0; i < 3; i++)
        {
            UI_Selection selection = Managers.UI.AddSubItemUI<UI_Selection>(_selections.transform);
            int rand = Random.Range(0, range);
            if (rand < (int)Define.WeaponType.MaxCount) selection.SetType((Define.WeaponType)rand);
            else selection.SetType((Define.StatType)(rand - Define.WeaponType.MaxCount));
        }
    }
}
