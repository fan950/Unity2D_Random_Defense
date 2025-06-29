using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICompleted : UIBase
{
    public GameObject[] arrStarObj;

    private const string sCloseEvent = "Close";

    public void Set_Star(int nLifeCount)
    {
        for (int i = 0; i < arrStarObj.Length; ++i)
        {
            arrStarObj[i].SetActive(i < nLifeCount);
        }
    }
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
}
