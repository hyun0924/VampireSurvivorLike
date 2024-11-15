using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Layer
    {
        Tile = 3,
        Player = 6,
        Bullet,
        Enemy,
        Dead,
        Exp,
    }

    public enum EnemyType
    {
        Enemy_0,
        Enemy_1,
        Enemy_2,
        Enemy_3,
        Enemy_4,
    }

    public enum State
    {
        Idle,
        Run,
        Hit,
        Dead,
    }

    public enum DataName
    {
        ExpStats,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    // 박스에서 나오는 것만 Box 밑에 두기
    public enum Prop
    {
        Exp,
        Box,
        Magnet,
        Heal,
        BoxEnd,
    }
}
