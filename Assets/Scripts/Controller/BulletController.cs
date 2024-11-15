using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : WeaponController
{
    [SerializeField] private float _shotSpeed = 5f;
    [SerializeField] private Vector3 _direction = Vector3.zero;

    public void Init(Transform parent, GameObject target)
    {
        _parent = parent;
        Type = BulletType.Bullet;

        if (target == null)
        {
            Managers.Resource.Destroy(gameObject);
            return;
        }
        _direction = target.transform.position - parent.transform.position;

        transform.position = parent.position;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Vector2.SignedAngle(Vector3.up, _direction)));
    }

    // Update is called once per frame

    private void Update()
    {
        transform.position += _direction.normalized * _shotSpeed * Time.deltaTime;

        if ((transform.position - _parent.position).magnitude > 20f)
            Managers.Resource.Destroy(gameObject);
    }
}
