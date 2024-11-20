using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelController : WeaponController
{
    [SerializeField] private float _degree = 0f;
    [SerializeField] private float _rotateSpeed = 120f;
    [SerializeField] private float _radius = 1f;

    public void Init(float degree)
    {
        _parent = transform.parent;
        Type = BulletType.Shovel;
        _degree = degree;
    }

    private void Update()
    {
        float rotateSpeed = _rotateSpeed * Managers.Game.Player.StatPlayer.Cooldown;

        _degree += rotateSpeed * Time.deltaTime;
        transform.localPosition = new Vector3(Mathf.Sin(_degree * Mathf.Deg2Rad), Mathf.Cos(_degree * Mathf.Deg2Rad)) * _radius;

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation = new Vector3(rotation.x, rotation.y, -_degree);
        transform.rotation = Quaternion.Euler(rotation);
    }
}
