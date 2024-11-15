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

        gameObject.GetOrAddComponent<SpawnPool>().Init(10);
    }
}
