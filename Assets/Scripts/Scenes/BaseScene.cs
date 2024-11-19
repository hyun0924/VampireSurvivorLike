using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        if (GameObject.Find("EventSystem") == null)
            Managers.Resource.Instantiate("@EventSystem");
    }

    public abstract void Clear();
}