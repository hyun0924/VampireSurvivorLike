using UnityEngine;

public class UI_CharacterSelect : UI_Popup
{
    GameObject _characterSelections;

    enum GameObjects
    {
        CharacterSelections
    }

    protected override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        _characterSelections = GetGameObject((int)GameObjects.CharacterSelections);

        foreach (var info in Managers.Data.PlayerInfoDict)
        {
            UI_CharacterSelection ui = Managers.UI.AddSubItemUI<UI_CharacterSelection>(_characterSelections.transform);
            ui.SetInfo(info.Value);
        }
    }
}