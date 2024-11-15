using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPool : MonoBehaviour
{
    List<SpawnTarget> _spawnTargets = new List<SpawnTarget>();
    GameObject _root { get { return Util.GetOrCreateGameObject("@Live_Pool"); } }

    class SpawnTarget
    {
        public string Name;
        public Transform Pool;
        public int CurrentCount = 0;
        public int ReservedCount = 0;
        public int MaxCount = 5;
    }

    public void AddSpawnTarget(string prefabName, int count)
    {
        SpawnTarget target = new SpawnTarget();
        target.Name = prefabName;
        target.MaxCount = count;
        target.Pool = new GameObject { name = $"{prefabName}_Live_Pool" }.transform;
        target.Pool.parent = _root.transform;

        _spawnTargets.Add(target);
    }

    private void Update()
    {
        foreach (SpawnTarget target in _spawnTargets)
        {
            target.CurrentCount = target.Pool.childCount;
            if (target.ReservedCount + target.CurrentCount < target.MaxCount)
            {
                StartCoroutine(ReserveSpawn(target));
                target.ReservedCount++;
            }
        }
    }

    IEnumerator ReserveSpawn(SpawnTarget target)
    {
        yield return new WaitForSeconds(Random.Range(0, 5.0f));
        GameObject go = Managers.Resource.Instantiate(target.Name, target.Pool);

        float degree = Random.Range(0, 360.0f);
        float distance = Random.Range(10f, 15f);
        Vector3 randPos = Managers.Game.Player.transform.position + new Vector3(Mathf.Cos(degree * Mathf.Deg2Rad), Mathf.Sin(degree * Mathf.Deg2Rad)) * distance;
        
        IPoolable poolable = go.GetComponent<IPoolable>();
        if (poolable != null)
            poolable.Respawn(randPos);
        else
            go.transform.position = randPos;

        target.CurrentCount++;
        target.ReservedCount--;
    }
}
