using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        return Util.GetOrAddComponent<T>(go);
    }
    public static T FindChild<T>(this GameObject parent, string name) where T : Object
    {
        return Util.FindChild<T>(parent, name);
    }
}
