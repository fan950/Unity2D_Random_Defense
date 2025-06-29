using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOpening : UIBase
{
    private IGameInfo gameInfo;

    public void Set_Interface(IGameInfo gameInfo)
    {
        this.gameInfo = gameInfo;
    }
    public override void Close()
    {
        gameInfo.Set_Pause(false);
        gameObject.SetActive(false);
    }
    public override void Open()
    {
        gameObject.SetActive(true);
    }
}
