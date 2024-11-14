using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.position = new Vector3(Managers.Game.Player.transform.position.x, Managers.Game.Player.transform.position.y, -10);
    }
}