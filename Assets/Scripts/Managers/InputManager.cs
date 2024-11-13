using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public Action NoInputAction;
    public Action KeyBoardAction;

    public void UpdateInput()
    {
        if (!Input.anyKey)
        {
            if (NoInputAction != null) NoInputAction.Invoke();
            return;
        }

        if (KeyBoardAction != null)
            KeyBoardAction.Invoke();
    }
}
