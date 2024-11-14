using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Util
{
    public static GameObject GetOrCreateGameObject(string name, Transform parent = null)
    {
        GameObject go = GameObject.Find(name);
        if (go == null)
        {
            go = new GameObject { name = name };
            go.transform.parent = parent;
        }

        return go;
    }

    public static T GetOrAddComponent<T> (GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }
        return component;
    }

    public static T FindChild<T>(GameObject parent, string name) where T : Object
    {
        T[] components = parent.GetComponentsInChildren<T>();
        foreach (T component in components)
        {
            if (component.name == name)
            {
                return component;
            }
        }

        return null;
    }
}
