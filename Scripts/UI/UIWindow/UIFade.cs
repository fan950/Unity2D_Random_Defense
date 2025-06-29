using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    private Coroutine onCoro;
    private Coroutine offCoro;
    public Image fadeImg;

    public void OnFade(Action action)
    {
        gameObject.SetActive(true);

        if (onCoro == null)
            onCoro = StartCoroutine(Play_OnFade(action));
    }

    public void OffFade(Action action)
    {
        gameObject.SetActive(true);

        if (offCoro == null)
            offCoro = StartCoroutine(Play_OffFade(action));
    }
    private IEnumerator Play_OffFade(Action action)
    {
        fadeImg.color = new Color(0, 0, 0, 1);
        float _fTime = 1;
        while (true)
        {
            yield return null;
            fadeImg.color = new Color(0, 0, 0, _fTime);
            _fTime -= Time.deltaTime * 2;

            if (_fTime <= 0)
            {
                if (action != null)
                    action();
                gameObject.SetActive(false);
                offCoro = null;
                break;
            }
        }
    }
    private IEnumerator Play_OnFade(Action action)
    {
        fadeImg.color = new Color(0, 0, 0, 0);
        float _fTime = 0;
        while (true)
        {
            yield return null;
            fadeImg.color = new Color(0, 0, 0, _fTime);
            _fTime += Time.deltaTime * 2;

            if (_fTime >= 1)
            {
                if (action != null)
                    action();
                onCoro = null;
                break;
            }
        }
    }
}
