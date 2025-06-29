using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private IUIEventManager uiButtonEventManager;
    private const string sClickEvent = "Click";
    public void Init(IUIEventManager uIButtonEventManager)
    {
        uiButtonEventManager = uIButtonEventManager;
    }

    public void Update_Mgr()
    {
        if (Input.GetMouseButtonDown(0))
        {
            uiButtonEventManager.Trigger(sClickEvent);
        }
    }
}
