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
        go.transform.position = Managers.Game.Player.transform.position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * 15f;

        _currentCount++;
        _reservedCount--;
    }
}
