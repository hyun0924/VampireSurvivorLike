using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UI_Game;

public class GameScene : BaseScene
{
    int _maxEnemyCount = 10;

    protected override void Init()
    {
        base.Init();
        Managers.TileMap.Init();

        if (!Managers.Sound.IsPlaying(Define.AudioSource.BGM))
            Managers.Sound.Play(Define.Audio.BGM, Define.AudioSource.BGM);

        UI_Game gameUI = Managers.UI.ShowSceneUI<UI_Game>();

        if (Managers.Game.CurrentPlayerType == Define.Player.Unknown)
            Managers.Game.CurrentPlayerType = Define.Player.Farmer_0;

        if (GameObject.Find("Player") == null)
            Managers.Game.Player = Managers.Resource.Instantiate("Player").GetOrAddComponent<PlayerController>();

        SpawnPool spawnPool = gameObject.GetOrAddComponent<SpawnPool>();
        spawnPool.AddSpawnTarget(Enum.GetName(typeof(Define.Enemy), Define.Enemy.Enemy_0), _maxEnemyCount);
        spawnPool.AddSpawnTarget(Enum.GetName(typeof(Define.Prop), Define.Prop.Box), 2, 30f);

        StartCoroutine(UpdateTime(gameUI));
    }

    IEnumerator UpdateTime(UI_Game gameUI)
    {
        Managers.Game.GameSecond = 0;
        WaitForSeconds sec = new(1f);
        while (true)
        {
            yield return sec;
            Managers.Game.GameSecond++;
            gameUI.SetText(Texts.TimeText, string.Format("{0:00}:{1:00}", Managers.Game.GameSecond / 60, Managers.Game.GameSecond % 60));

            if (Managers.Game.GameSecond == 60 * 30)
            {
                Managers.UI.ShowPopupUI<UI_GameClear>();
                Managers.Game.IsClear = true;
            }
        }
    }

    public override void Clear()
    {
        Managers.Instance.Clear();
    }
}
