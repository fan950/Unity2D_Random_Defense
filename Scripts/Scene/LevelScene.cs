using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScene : MonoBehaviour
{
    public UILevel uiLevel;
    public void Awake()
    {
        uiLevel.Init(UIEventManager.Instance);
        uiLevel.Open();

        UIManager.Instance.Fade(false);
    }
}
