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

        List<int> availableSelections = new List<int>();
        for (int i = 0; i < (int)Define.PlayerLevel.MaxCount; i++)
            if (Managers.Game.Player.Levels[i] != 6) // Max Level 6
                availableSelections.Add(i);

        for (int i = 0; i < 3; i++)
        {
            if (availableSelections.Count == 0)
            {
                Managers.UI.ClosePopupUI();
                break;
            }

            UI_UpgradeSelection selection = Managers.UI.AddSubItemUI<UI_UpgradeSelection>(_selections.transform);
            int rand = availableSelections[Random.Range(0, availableSelections.Count)];
            selection.SetInfo((Define.PlayerLevel)rand);

            availableSelections.Remove(rand);
        }
    }
}
