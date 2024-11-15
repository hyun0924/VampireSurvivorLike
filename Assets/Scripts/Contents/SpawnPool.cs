using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPool : MonoBehaviour
{
    [SerializeField] int _currentCount = 0;
    [SerializeField] int _reservedCount = 0;
    [SerializeField] int _maxCount = 5;

    GameObject _root;

    public void Init(int count)
    {
        _maxCount = count;
        _root = Util.GetOrCreateGameObject("@Enemy0_LivePool");
    }

    private void Update()
    {
        _currentCount = _root.transform.childCount;
        if (_reservedCount + _currentCount < _maxCount)
        {
            StartCoroutine("ReserveSpawn");
            _reservedCount++;
        }
    }

    IEnumerator ReserveSpawn()
    {
        yield return new WaitForSeconds(Random.Range(0, 5.0f));
        GameObject go = Managers.Resource.Instantiate("Enemy_0", _root.transform);

        float degree = Random.Range(0, 360.0f);
        float distance = Random.Range(10f, 15f);
        go.transform.position = Managers.Game.Player.transform.position + new Vector3(Mathf.Cos(degree * Mathf.Deg2Rad), Mathf.Sin(degree * Mathf.Deg2Rad)) * distance;

        _currentCount++;
        _reservedCount--;
    }
}
