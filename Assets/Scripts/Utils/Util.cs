using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Transform = UnityEngine.Transform;

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

    public static GameObject FindNearestEnemy(Vector3 position, float range)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, range, (1 << (int)Define.Layer.Enemy));

        if (colliders.Length == 0)
            return null;

        GameObject enemy = null;
        float minDistance = float.MaxValue;
        foreach (Collider2D collider in colliders)
        {
            float distance = (collider.transform.position - position).sqrMagnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                enemy = collider.gameObject;
            }
        }

        return enemy;
    }

    public static GameObject FindNearestEnemy(GameObject go, float range)
    {
        return FindNearestEnemy(go.transform.position, range);
    }
}
