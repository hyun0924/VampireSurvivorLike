using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    SpriteRenderer _sr;
    GameObject _target = null;
    [SerializeField] float _speed = 3f;
    [SerializeField] float _detectDistance = 15f;
    [SerializeField] float _hitDuration = 0.2f;
    bool _isAttacked = false;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_target == null)
            _target = Physics2D.OverlapCircle(transform.position, _detectDistance, LayerMask.GetMask("Player"))?.gameObject;
        else
        {
            Vector3 dest = _target.transform.position;
            Vector3 dir = dest - transform.position;

            if (dir.magnitude > _detectDistance + 1f)
                transform.position += dir * 2;

            _sr.flipX = (dir.x < 0);
            transform.position += dir.normalized * _speed * Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == (int)Define.Layer.Bullet && !_isAttacked)
        {
            WeaponController controller = other.GetComponent<WeaponController>();
            StartCoroutine(OnHit(controller.Damage));

            if (controller.Type == WeaponController.BulletType.Bullet)
                Destroy(other.gameObject);
        }
    }

    IEnumerator OnHit(int damage)
    {
        _isAttacked = true;
        Color originColor = _sr.color;
        _sr.color = new Color32(255, 134, 134, 255);
        Vector3 dir = transform.position - _target.transform.position;
        transform.position += dir.normalized * 0.5f;
        yield return new WaitForSeconds(_hitDuration);
        _sr.color = originColor;
        _isAttacked = false;
    }
}
