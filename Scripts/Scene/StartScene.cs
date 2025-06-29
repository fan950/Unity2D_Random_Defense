using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    private const string sStartEvent = "Start";
    private const string sLevelSceneEvent = "LevelScene";
    public void Start()
    {
        UIEventManager.Instance.Subscribe(sStartEvent, LevelScene);
    }
    public void LevelScene()
    {
        UIEventManager.Instance.Unsubscribe(sStartEvent, LevelScene);

        NextScene(sLevelSceneEvent);
    }
    public void NextScene(string sNext)
    {
        UIManager.Instance.Fade(true, delegate
        {
            UIManager.Instance.NextScene_UI();
            Resources.UnloadUnusedAssets();

            SceneManager.LoadScene(sNext);
        });
    }
}
