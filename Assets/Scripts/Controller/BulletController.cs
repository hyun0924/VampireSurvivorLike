using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : WeaponController
{
    private float _shotSpeed;
    private Vector3 _direction = Vector3.zero;
    private int _count; // °üÅë

    public void Init(Transform parent, Vector3 pos, int damage = 6, int count = 1, float speed = 7f)
    {
        Type = BulletType.Bullet;
        _parent = parent;
        _direction = pos - parent.transform.position;
        _damage = damage;
        _count = count;
        _shotSpeed = speed;

        transform.position = parent.position;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Vector2.SignedAngle(Vector3.up, _direction)));
    }

    private void Update()
    {
        transform.position += _direction.normalized * _shotSpeed * Time.deltaTime;

        if ((transform.position - _parent.position).magnitude > 20f)
            Managers.Resource.Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)Define.Layer.Enemy)
        {
            _count--;

            if (_count == 0)
                Managers.Resource.Destroy(gameObject);
        }
    }
}
