using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T> (string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject origin = Load<GameObject>($"Prefabs/{path}");
        GameObject go = null;
        if (origin != null)
        {
            go = Object.Instantiate(origin, parent);
            go.transform.SetParent(parent);
            go.name = origin.name;
        }
        return go;
    }
}
