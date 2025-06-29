using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IGameInfo
{
    public void Set_Life(int nLife);
    public void Set_Pause(bool bPause);
    public void Set_Gold(int nGold);
    public int Get_Life();
    public int Get_Gold();
    public float Get_Time();
    public GameObject[] Get_WayPoint();
}
public class GameScene : MonoBehaviour, IGameInfo
{
    public bool bPause;
    public int nWaveIndex;
    public GameObject[] arrRouteObj;

    private GameData gameData;
    private WaveData waveData;

    private int nLife;
    private int nGold;
    private float fWaveTime;
    private float fEndTime;
    private bool bClear;

    private const string sReGameEvent = "ReGame";
    private const string sBackEvent = "Back";
    private const string sShowTimeEvent = "ShowTime";
    private const string sShowGoldEvent = "ShowGold";
    private const string sShowLifeEvent = "ShowLife";
    private const string sLevelSceneEvent = "LevelScene";

    private const string sCompletedPath = "UI/UICompleted";
    private const string sDefeatPath = "UI/UIDefeat";
    private const string sOpeningPath = "UI/UIOpening";
    private const string sScreenPath = "UI/UIScreen";
    private const string sHUDPath = "UI/UIHUD";
    private const string sWaveTablePath = "Table/WaveTable";
    public void Awake()
    {
        IGameInfo gameInfo = this;
        bPause = true;
        gameData = SaveManager.Instance.Load();
        UIHUD _uiHUD = UIManager.Instance.GetUI(sHUDPath) as UIHUD;
        _uiHUD.Set_CombatMgr(CombatManager.Instance);
        _uiHUD.Open();

        UIScreen _uiScreen = UIManager.Instance.GetUI(sScreenPath) as UIScreen;
        _uiScreen.Set_Interface(gameInfo, TowerManager.Instance, SkillManager.Instance);
        _uiScreen.Open();

        InputManager.Instance.Init(UIEventManager.Instance);
        TowerManager.Instance.Set_Tower();

        WaveDataTable _waveTable = Resources.Load(sWaveTablePath) as WaveDataTable;
        _waveTable.Init();

        waveData = _waveTable.Get_Data(nWaveIndex);
        MonsterManager.Instance.Set_Wave(gameInfo, arrRouteObj, waveData);
        GuardManager.Instance.Set_Guard();

        fWaveTime = 0;

        CombatManager.Instance.Set_Combat(TowerManager.Instance, MonsterManager.Instance, BulletManager.Instance);
        CombatManager.Instance.dieMonsterAction = (nGold) => { Set_Gold(nGold); };
        nGold = 0;
        fEndTime = waveData.fEndTime;

        Set_Gold(waveData.nGold);
        Set_Life(waveData.nLife);

        UIOpening _uiOpening = UIManager.Instance.GetUI(sOpeningPath) as UIOpening;
        _uiOpening.transform.SetAsLastSibling();
        _uiOpening.Set_Interface(gameInfo);
        _uiOpening.Open();

        UIManager.Instance.Fade(false);

        UIEventManager.Instance.Subscribe(sReGameEvent, ReGameScene);
        UIEventManager.Instance.Subscribe(sBackEvent, BackScene);
    }

    public void Update()
    {
        if (bClear || bPause)
            return;

        InputManager.Instance.Update_Mgr();
        TowerManager.Instance.Update_Mgr();
        MonsterManager.Instance.Update_Mgr();
        BulletManager.Instance.Update_Mgr();
        GuardManager.Instance.Update_Mgr();
        SkillManager.Instance.Update_Mgr();
        CombatManager.Instance.Update_Mgr();

        fEndTime -= Time.deltaTime;
        fWaveTime += Time.deltaTime;

        if (fEndTime >= 0)
            UIEventManager.Instance.Trigger(sShowTimeEvent);

        if (fWaveTime >= waveData.fCoolTimeDown)
        {
            fWaveTime = 0;

            int _nRandom = Random.Range(0, waveData.nArrMonsterIndex.Length);
            MonsterManager.Instance.Create_Monster(waveData.nArrMonsterIndex[_nRandom]);
        }

        if (fEndTime <= 0)
        {
            UICompleted _uICompleted = UIManager.Instance.GetUI(sCompletedPath) as UICompleted;
            _uICompleted.Set_Star(nLife);
            bClear = true;

            if (gameData.lisStar.Count >= nWaveIndex)
            {
                gameData.lisStar[nWaveIndex - 1] = nLife;
            }
            else
            {
                gameData.lisStar.Add(nLife);
            }
            SaveManager.Instance.Save(gameData);
        }
    }
    public void Set_Gold(int nGold)
    {
        this.nGold += nGold;
        UIEventManager.Instance.Trigger(sShowGoldEvent);
    }

    public int Get_Life()
    {
        return nLife;
    }
    public int Get_Gold()
    {
        return nGold;
    }
    public float Get_Time()
    {
        return fEndTime;
    }
    public GameObject[] Get_WayPoint()
    {
        return arrRouteObj;
    }
    public void Set_Pause(bool bPause)
    {
        this.bPause = bPause;
    }

    public void Set_Life(int nLife)
    {
        this.nLife = nLife;
        UIEventManager.Instance.Trigger(sShowLifeEvent);

        if (this.nLife <= 0)
        {
            bPause = true;
            UIManager.Instance.GetUI(sDefeatPath);
        }
    }

    private void ReGameScene()
    {
        NextScene(SceneManager.GetActiveScene().name);
    }
    public void BackScene()
    {
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
