using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHpbar : MonoBehaviour
{
    public bool bActive;

    public Image hpImg;
    private RectTransform rectTransform;
    [HideInInspector] public GameObject targetObj;

    private float fLifeTime;
    private const float fLifeMax = 3;
    public void Init()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
    }
    public void Set_Hpbar(GameObject target, int nMax, int nHp)
    {
        bActive = true;
        targetObj = target;

        fLifeTime = 0;
        hpImg.fillAmount = nHp / (nMax * 1.0f);
    }
    public void Update_UI()
    {
        fLifeTime += Time.deltaTime;
        if (fLifeTime >= fLifeMax)
        {
            bActive = false;
        }

        if (targetObj != null)
        {
            rectTransform.anchoredPosition = UIManager.Instance.Get_UIPos(targetObj.transform.position);
        }
    }
}
