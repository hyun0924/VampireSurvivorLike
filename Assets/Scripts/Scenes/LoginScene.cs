using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        Managers.TileMap.Init();
        Managers.UI.ShowSceneUI<UI_Login>();
    }

    public override void Clear()
    {
        Managers.TileMap.Clear();
    }
}
