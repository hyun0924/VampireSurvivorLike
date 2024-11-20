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

        if (!Managers.Sound.IsPlaying(Define.AudioSource.BGM))
            Managers.Sound.Play(Define.Audio.BGM, Define.AudioSource.BGM, 0.4f);
    }

    public override void Clear()
    {
        Managers.TileMap.Clear();
        Managers.UI.Clear();
    }
}
