using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    [SerializeField] int _maxEnemyCount = 10;


    protected override void Init()
    {
        base.Init();
        Managers.TileMap.Init();

        UI_Game gameUI = Managers.UI.ShowSceneUI<UI_Game>();

        Managers.Game.CurrentPlayerType = Define.PlayerType.Farmer_0;
        Managers.Game.Player = Managers.Resource.Instantiate("Player").GetOrAddComponent<PlayerController>();

        SpawnPool spawnPool = gameObject.GetOrAddComponent<SpawnPool>();
        spawnPool.AddSpawnTarget(Enum.GetName(typeof(Define.EnemyType), Define.EnemyType.Enemy_0), _maxEnemyCount);
        spawnPool.AddSpawnTarget(Enum.GetName(typeof(Define.Prop), Define.Prop.Box), 2, 30f);
    }

    public override void Clear()
    {
        Managers.Instance.Clear();
    }
}
