using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillSlot : UIBtn
{
    private float fTime;
    private float fCoolTimeDown;
    public string sIndexName;
    public GameObject activeObj;
    public Image coolTimeDownImg;
    public Text coolTimeDownTxt;

    public override void Start()
    {
        base.Start();
        activeObj.SetActive(false);
        coolTimeDownImg.gameObject.SetActive(false);

        fCoolTimeDown = SkillManager.Instance.Get_SkillData(sIndexName).nCoolTimeDown;
    }

    public override void OnClick()
    {
        if (coolTimeDownImg.gameObject.activeSelf)
            return;

        UIEventManager.Instance.Trigger(sEventId, sIndexName);
        fTime = fCoolTimeDown;
        coolTimeDownTxt.text = fCoolTimeDown.ToString();
        coolTimeDownImg.fillAmount = 1;
        coolTimeDownImg.gameObject.SetActive(true);
    }

    public void Update()
    {
        if (coolTimeDownImg.gameObject.activeSelf)
        {
            fTime -= Time.deltaTime;
            coolTimeDownImg.fillAmount = fTime / fCoolTimeDown;
            coolTimeDownTxt.text = Mathf.FloorToInt(fTime+1).ToString();
            if (fTime <= 0)
            {
                coolTimeDownImg.gameObject.SetActive(false);
            }
        }
    }
}
