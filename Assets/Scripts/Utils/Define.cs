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
        Shovel,
    }

    public enum Player
    {
        Farmer_0,
        Farmer_1,
        Farmer_2,
        MaxCount,
        Unknown
    }

    public enum Enemy
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

    public enum PlayerLevel
    {
        Shovel,
        Gun,
        Power,
        Speed,
        MaxHP,
        Cooldown,
        MaxCount
    }

    public enum Shovel
    {
        Shovel,
        Pitchfork,
        Sickle
    }

    public enum Gun
    {
        Riple,
        Sniper,
        Shotgun,
    }

    public enum Scene
    {
        Login,
        Game,
    }

    public enum PlayerSprite
    {
        Dead_0, Dead_1,
        Run_0, Run_1, Run_2, Run_3, Run_4, Run_5,
        Stand_0, Stand_1, Stand_2, Stand_3
    }

    public enum AudioSource
    {
        BGM,
        SFX,
        MaxCount
    }

    public enum Audio
    {
        BGM,
        Dead,
        Hit0, Hit1,
        LevelUp,
        Win, Lose,
        Melee0, Melee1,
        Range,
        Select
    }
}
