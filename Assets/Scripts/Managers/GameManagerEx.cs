using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    public PlayerController Player = null;
    public GameObject Exps { get { return Util.GetOrCreateGameObject("@Exps"); } }
}
