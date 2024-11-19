using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PoolManager
{
    class Pool
    {
        public GameObject Origin = null;
        public Transform Root = null;

        Stack<Poolable> _stack = new Stack<Poolable>();

        public void Init(GameObject origin, int count)
        {
            Origin = origin;
            Root = Util.GetOrCreateGameObject($"{Origin.name}_Pool").transform;

            for (int i = 0; i < count; i++)
            {
                Push(CreateClone());
            }
        }

        private GameObject CreateClone()
        {
            GameObject go = Object.Instantiate(Origin);
            go.name = Origin.name;
            return go;
        }

        public Poolable Pop(Transform parent = null)
        {
            GameObject go = null;

            if (_stack.Count > 0)
            {
                go = _stack.Pop().gameObject;
                go.SetActive(true);
            }
            else
                go = CreateClone();

            if (parent == null) go.transform.parent = GameObject.Find("@Scene").transform;
            go.transform.parent = parent;

            return go.GetOrAddComponent<Poolable>();
        }

        public void Push(GameObject go)
        {
            go.transform.parent = Root.transform;
            go.SetActive(false);
            Poolable poolable = go.GetOrAddComponent<Poolable>();
            _stack.Push(poolable);
        }
    }

    Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();
    private GameObject Root = null;

    public void Init()
    {
        Root = Util.GetOrCreateGameObject($"@Pool_Root");
        Object.DontDestroyOnLoad(Root);
    }

    private void CreatePool(GameObject origin, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(origin, count);
        pool.Root.parent = Root.transform;

        _pools.Add(origin.name, pool);
    }

    public GameObject Pop(GameObject origin, Transform parent = null)
    {
        if (!_pools.ContainsKey(origin.name))
            CreatePool(origin);

        return _pools[origin.name].Pop(parent).gameObject;
    }

    public void Push(GameObject go)
    {
        if (!_pools.ContainsKey(go.name))
            Object.Destroy(go);
        else
            _pools[go.name].Push(go);
    }

    public GameObject GetOrigin(string name)
    {
        if (name.Contains("/"))
            name = name.Substring(name.IndexOf("/") + 1);

        GameObject go = null;
        if (_pools.ContainsKey(name))
            go = _pools[name].Origin;

        return go;
    }

    public void Clear()
    {
        _pools.Clear();
        for (int i = 0; i < Root.transform.childCount; i++)
            Object.Destroy(Root.transform.GetChild(i).gameObject);
    }
}