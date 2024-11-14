using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : UnityEngine.Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            GameObject go = Managers.Pool.GetOrigin(path);
            if (go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject origin = Load<GameObject>($"Prefabs/{path}");
        if (origin == null) return null;

        GameObject go = null;

        if (origin.GetComponent<Poolable>() != null)
        {
            go = Managers.Pool.Pop(origin, parent);
        }
        else
        {
            go = UnityEngine.Object.Instantiate(origin, parent);
            go.transform.SetParent(parent);
            go.name = origin.name;
        }

        return go;
    }

    public Sprite LoadSubSprite(string imageName, string spriteName)
    {
        Sprite sprite = null;

        Sprite[] subSprites = Resources.LoadAll<Sprite>($"Arts/Sprites/{imageName}");
        foreach (Sprite subSprite in subSprites)
        {
            if (subSprite.name == spriteName)
                sprite = subSprite;
        }

        return sprite;
    }

    public void Destroy(GameObject go)
    {
        if (go.GetComponent<Poolable>() != null) Managers.Pool.Push(go);
        else UnityEngine.Object.Destroy(go);
    }
}
