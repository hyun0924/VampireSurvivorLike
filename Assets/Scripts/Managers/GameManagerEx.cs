using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    public PlayerController Player = null;

    public Define.PlayerType CurrentPlayerType = Define.PlayerType.Unknown;
    public GameObject Props { get { return Util.GetOrCreateGameObject("@Props"); } }
    public int Second;

    public void Clear()
    {
        CurrentPlayerType = Define.PlayerType.Unknown;
        Second = 0;
    }
}
