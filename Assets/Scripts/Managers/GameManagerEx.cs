using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    public Define.Player CurrentPlayerType = Define.Player.Unknown;
    public PlayerController Player = null;
    public GameObject Props { get { return Util.GetOrCreateGameObject("@Props"); } }
    public int GameSecond;
    public bool[] IsLocked = new bool[(int)Define.Player.MaxCount];
    public bool IsClear = false;

    public void Init()
    {
        for (int i = 0; i < IsLocked.Length; i++)
            IsLocked[i] = true;
        IsLocked[0] = false;
    }

    public void Clear()
    {
        CurrentPlayerType = Define.Player.Unknown;
        Player = null;
        GameSecond = 0;
        IsClear = false;
    }
}
