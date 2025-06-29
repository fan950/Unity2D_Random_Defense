using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIDefeat : UIBase
{
    private const string sCloseEvent = "Close";
    private const string sReGameEvent = "ReGame";
    private const string sBackEvent = "Back";

    public override void Open()
    {
        uiEventManager.Subscribe(sCloseEvent, Close);

        gameObject.SetActive(true);
    }
    public override void Close()
    {
        uiEventManager.Unsubscribe(sCloseEvent, Close);
        gameObject.SetActive(false);
    }
    public void ReGame() 
    {
        uiEventManager.Trigger(sReGameEvent);
    }
    public void Back()
    {
        uiEventManager.Trigger(sBackEvent);
    }
}
