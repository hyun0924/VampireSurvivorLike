using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : WeaponController
{
    [SerializeField] private float _range = 5f;
    [SerializeField] private float _shotSpeed = 5f;
    [SerializeField] private Vector3 _direction = Vector3.zero;

    public void Init(Transform parent)
    {
        _parent = parent;
        Type = BulletType.Bullet;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(parent.transform.position, _range, (1 << (int)Define.Layer.Enemy));

        if (colliders.Length == 0)
        {
            Destroy(gameObject);
            return;
        }

        float minDistance = float.MaxValue;
        foreach (Collider2D collider in colliders)
        {
            Vector3 dir = collider.transform.position - parent.transform.position;
            float distance = dir.sqrMagnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                _direction = dir.normalized;
            }
        }

        transform.position = parent.position;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Vector2.SignedAngle(Vector3.up, _direction)));
    }

    // Update is called once per frame

    private void Update()
    {
        transform.position += _direction.normalized * _shotSpeed * Time.deltaTime;

        if ((transform.position - _parent.position).magnitude > 20f)
            Destroy(gameObject);
    }
}
