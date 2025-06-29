using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UILevel : UIBase
{
    private IUIEventManager uiButtonEventManager;
    public GameObject lockObj;
    public Image mapImg;
    public Text mapTxt;

    public Image[] arrStar;
    public Sprite unStar;
    public Sprite Star;

    private string sScene;
    private int nIndex;
    private MapDataTable mapTable;
    private GameData gameData;
    private const string sMapPath = "Table/MapTable";
    private const string sNextEvent = "Next";
    private const string sPrevEvent = "Prev";
    private const string sStartEvent = "Start";
    public override void Init(IUIEventManager uIButtonEventManager)
    {
        UIManager.Instance.Fade(false);

        gameData = SaveManager.Instance.Load();
        uiButtonEventManager = uIButtonEventManager;
        mapTable = Resources.Load(sMapPath) as MapDataTable;
        nIndex = 1;

        for (int i = 0; i < arrStar.Length; ++i)
        {
            arrStar[i].sprite = unStar;
        }
    }

    public override void Open()
    {
        Set_Map(0);

        uiButtonEventManager.Subscribe(sNextEvent, NextBtn);
        uiButtonEventManager.Subscribe(sPrevEvent, PrevBtn);
        uiButtonEventManager.Subscribe(sStartEvent, InGameBtn);
    }

    public override void Close()
    {
        uiButtonEventManager.Unsubscribe(sNextEvent, NextBtn);
        uiButtonEventManager.Unsubscribe(sPrevEvent, PrevBtn);
        uiButtonEventManager.Unsubscribe(sStartEvent, InGameBtn);
    }

    public void NextBtn()
    {
        Set_Map(1);
    }
    public void PrevBtn()
    {
        Set_Map(-1);
    }

    public void InGameBtn()
    {
        if (lockObj.activeSelf)
            return;

        Close();

        SceneManager.LoadScene(sScene);
    }
    public void Set_Map(int nNext)
    {
        int _nIndex = nIndex + nNext;

        MapData _data = mapTable.Get_Map(_nIndex);

        if (_data == null)
            return;

        if (gameData.lisStar.Count + 1 >= _nIndex)
        {
            lockObj.SetActive(false);

            if (gameData.lisStar.Count + 1 == _nIndex)
            {
                for (int i = 0; i < arrStar.Length; ++i)
                {
                    arrStar[i].sprite = unStar;
                }
            }
            else
            {
                for (int i = 0; i < gameData.lisStar[_nIndex - 1]; ++i)
                {
                    arrStar[i].sprite = Star;
                }
            }
        }
        else
        {
            lockObj.SetActive(true);
            for (int i = 0; i < arrStar.Length; ++i)
            {
                arrStar[i].sprite = unStar;
            }
        }

        mapImg.sprite = _data.mapSprite;
        mapTxt.text = _data.sName;

        nIndex = _nIndex;
        sScene = _data.sScene;
    }
}
