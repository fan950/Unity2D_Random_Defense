using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    protected IUIEventManager uiEventManager;
    public virtual void Init(IUIEventManager uIEventManager) 
    {
        uiEventManager = uIEventManager;
    }
    public abstract void Open();
    public abstract void Close();
}
