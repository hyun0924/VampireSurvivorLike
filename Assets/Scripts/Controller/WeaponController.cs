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

    protected int _damage = 6;
    protected Transform _parent = null;
    protected BulletType Type;

    public int GetDamage() { return _damage * Managers.Game.Player.Stat.Damage; }
}
