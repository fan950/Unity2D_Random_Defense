using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDamage : MonoBehaviour
{
    public bool bActive;

    public Text damageTxt;
    private RectTransform rectTransform;
    [HideInInspector]public GameObject targetObj;

    private float fColorTime;
    private float fLifeTime;
    private const float fLifeMax = 3;
    public void Init()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
    }

    public void Set_Damage(GameObject target, int nDamage)
    {
        damageTxt.rectTransform.anchoredPosition = Vector2.zero;
        damageTxt.color = Color.red;
        fColorTime = 1;

        bActive = true;
        targetObj = target;

        fLifeTime = 0;
        damageTxt.text = nDamage.ToString();
    }
    public void Update_UI()
    {
        damageTxt.color = new Color(1,0,0, fColorTime);

        fLifeTime += Time.deltaTime;
        fColorTime -= Time.deltaTime*0.8f;

        if (damageTxt.rectTransform.anchoredPosition.y < 30)
        {
            damageTxt.rectTransform.anchoredPosition += Vector2.up * (Time.deltaTime *80);
        }

        if (fLifeTime >= fLifeMax)
        {
            bActive = false;
            targetObj = null;

        }

        if (targetObj != null)
        {
            rectTransform.anchoredPosition = UIManager.Instance.Get_UIPos(targetObj.transform.position);
        }
    }
}
