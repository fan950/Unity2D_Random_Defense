using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBtn : MonoBehaviour
{
    public string sEventId;
    private Button btn;

    public virtual void Start()
    {
        if (btn == null)
            btn = GetComponent<Button>();

        btn.onClick.AddListener(OnClick);
    }

    public void Set_Button(bool bActive)
    {
        if (btn == null)
            btn = GetComponent<Button>();
        btn.interactable = bActive;
    }
    public virtual void OnClick()
    {
        UIEventManager.Instance.Trigger(sEventId);
    }
}
