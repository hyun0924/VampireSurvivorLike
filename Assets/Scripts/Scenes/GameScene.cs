using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    private void Awake()
    {
        Managers.Resource.Instantiate("@EventSystem");
        Managers.UI.ShowSceneUI<UI_Game>();

        Managers.Game.Player = Managers.Resource.Instantiate("Player").GetOrAddComponent<PlayerController>();

        SpawnPool spawnPool = gameObject.GetOrAddComponent<SpawnPool>();
        spawnPool.AddSpawnTarget(Enum.GetName(typeof(Define.EnemyType), Define.EnemyType.Enemy_0), 10);
        spawnPool.AddSpawnTarget(Enum.GetName(typeof(Define.Prop), Define.Prop.Box), 2);
    }
}
