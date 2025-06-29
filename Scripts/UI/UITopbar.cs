using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITopbar : MonoBehaviour
{
    public Text goldTxt;
    public Text lifeTxt;
    public Text timeTxt;

    public void Set_Gold(int nGold) 
    {
        goldTxt.text = nGold.ToString();
    }

    public void Set_Life(int nLife)
    {
        lifeTxt.text = string.Format("<size=50>x</size> {0}", nLife);
    }

    public void Set_Time(float fTime)
    {
        int minutes = Mathf.FloorToInt(fTime / 60f);
        int seconds = Mathf.FloorToInt(fTime % 60f);

        timeTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
