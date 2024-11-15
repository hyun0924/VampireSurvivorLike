using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    public PlayerController Player = null;
    public GameObject Props { get { return Util.GetOrCreateGameObject("@Props"); } }
}
