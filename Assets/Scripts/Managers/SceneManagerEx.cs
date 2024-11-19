using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindAnyObjectByType<BaseScene>(); } }

    public void LoadScene(Define.Scene scene)
    {
        CurrentScene.Clear();
        SceneManager.LoadScene(Enum.GetName(typeof(Define.Scene), scene));
    }
}
