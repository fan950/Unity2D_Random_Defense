using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class UIPaused : UIBase
{
    private Action action;

    private const string sCloseEvent = "Close";

    public void Set_Action(Action action)
    {
        this.action = action;
    }
    public override void Open()
    {
        uiEventManager.Subscribe(sCloseEvent, Close);
        gameObject.SetActive(true);
    }
    public override void Close()
    {
        uiEventManager.Unsubscribe(sCloseEvent, Close);
        action();
        gameObject.SetActive(false);
    }
}
