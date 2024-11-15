using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public enum BulletType
    {
        Shovel,
        Bullet,
    }

    public int Damage { get; protected set; } = 6;
    protected Transform _parent = null;
    public BulletType Type;
}
