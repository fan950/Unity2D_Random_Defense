using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Canvas canvas;
    [SerializeField] private UIFade uiFade;
    private RectTransform canvasRect;

    private Dictionary<string, UIBase> dicUI = new Dictionary<string, UIBase>();

    public override void Awake()
    {
        base.Awake();
        canvasRect = canvas.GetComponent<RectTransform>();
    }
    public Vector2 Get_UIPos(Vector3 pos)
    {
        Vector3 _screenPos = Camera.main.WorldToScreenPoint(pos);
        Vector2 _uiPos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, _screenPos, canvas.worldCamera, out _uiPos))
        {
            return _uiPos;
        }
        return Vector2.zero;
    }
    private UIBase Create(string sPath)
    {
        GameObject _instan = Instantiate(Resources.Load(sPath) as GameObject, canvas.transform);

        _instan.transform.localPosition = Vector3.zero;
        _instan.transform.localScale = Vector3.one;
        _instan.transform.localRotation = Quaternion.identity;
        UIBase _base = _instan.GetComponent<UIBase>();

        return _base;
    }
    public UIBase GetUI(string sPath)
    {
        if (!dicUI.ContainsKey(sPath))
        {
            var _getUI = Create(sPath);
            _getUI.Init(UIEventManager.Instance);
            dicUI.Add(sPath, _getUI);
        }

        return dicUI[sPath];
    }

    public void Fade(bool bOn, Action action = null)
    {
        if (bOn)
        {
            uiFade.OnFade(action);
        }
        else
        {
            uiFade.OffFade(action);
        }
    }
    public void NextScene_UI()
    {
        foreach (KeyValuePair<string, UIBase> _ui in dicUI)
        {
            if (_ui.Value.gameObject.activeSelf)
                _ui.Value.Close();
        }
    }
}
