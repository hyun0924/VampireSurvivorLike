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
            if (NoInputAction != null && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) NoInputAction.Invoke();
            return;
        }

        if (KeyBoardAction != null)
            KeyBoardAction.Invoke();
    }
}
