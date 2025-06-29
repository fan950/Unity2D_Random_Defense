using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIScreen : UIBase
{
    private UITopbar uiTopbar;
    public UIBtn upgradeBtn;

    public GameObject messageObj;
    private GameObject targetObj;
    private int nTargetLevel;

    private int nSkilllndex;
    public RectTransform buildRingPos;
    public RectTransform guardRingPos;


    private ITowerManager createTower;
    private IGameInfo gameInfo;
    private ISkillManager skillManager;

    private const string sCreateTowerEvent = "CreateTower";
    private const string sClickEvent = "Click";
    private const string sUpgradeEvent = "Upgrade";
    private const string sDestroyEvent = "Destroy";

    private const string sGoldEvent = "ShowGold";
    private const string sLifeEvent = "ShowLife";
    private const string sTimeEvent = "ShowTime";
    private const string sSkillEvent = "Skill";
    private const string sPausedOpen = "PausedOpen";

    private const string sUIPausedPath = "UI/UIPuased";

    public override void Init(IUIEventManager uIButtonEventManager)
    {
        base.Init(uIButtonEventManager);

        uiEventManager = uIButtonEventManager;

        buildRingPos.gameObject.SetActive(false);
        guardRingPos.gameObject.SetActive(false);

        messageObj.SetActive(false);

        uiTopbar = gameObject.GetComponentInChildren<UITopbar>();
    }
    public void Set_Interface(IGameInfo gameInfo, ITowerManager createTower, ISkillManager skillManager)
    {
        this.gameInfo = gameInfo;
        this.createTower = createTower;
        this.skillManager = skillManager;
    }
    public override void Open()
    {
        uiEventManager.Subscribe(sCreateTowerEvent, Create_Tower);
        uiEventManager.Subscribe(sUpgradeEvent, Upgrade_Tower);
        uiEventManager.Subscribe(sDestroyEvent, Destroy_Tower);

        uiEventManager.Subscribe(sClickEvent, Get_ClickObj);
        uiEventManager.Subscribe(sPausedOpen, Open_PausedPopup);

        uiEventManager.Subscribe(sGoldEvent, Set_Gold);
        uiEventManager.Subscribe(sLifeEvent, Set_Life);
        uiEventManager.Subscribe(sTimeEvent, Set_Time);
        uiEventManager.Subscribe(sSkillEvent, Set_Skill);
    }

    public override void Close()
    {
        uiEventManager.Unsubscribe(sCreateTowerEvent, Create_Tower);
        uiEventManager.Unsubscribe(sClickEvent, Get_ClickObj);
        uiEventManager.Unsubscribe(sPausedOpen, Open_PausedPopup);
        uiEventManager.Unsubscribe(sUpgradeEvent, Upgrade_Tower);
        uiEventManager.Unsubscribe(sDestroyEvent, Destroy_Tower);

        uiEventManager.Unsubscribe(sGoldEvent, Set_Gold);
        uiEventManager.Unsubscribe(sLifeEvent, Set_Life);
        uiEventManager.Unsubscribe(sTimeEvent, Set_Time);
        uiEventManager.Unsubscribe(sSkillEvent, Set_Skill);
    }
    public void Set_Skill(string sSkillName)
    {
        SkillData _skillData = skillManager.Get_SkillData(sSkillName);
        nSkilllndex = _skillData.nIndex;
        switch (_skillData.skillTarget)
        {
            case SkillTarget.Target:
                messageObj.SetActive(true);
                uiEventManager.Subscribe(sClickEvent, Skill_Point);
                break;
            case SkillTarget.NonTarget:
                var target = gameInfo.Get_WayPoint();
                for (int i = 0; i < _skillData.nCount; ++i)
                {
                    skillManager.Create_Skill(nSkilllndex, target[Random.Range(0, target.Length)].transform.position);
                }
                break;
        }
    }
    public void Create_Tower()
    {
        if (gameInfo.Get_Gold() < 10)
            return;

        gameInfo.Set_Gold(-10);
        createTower.Create_BaseTower();
    }
    public void Destroy_Tower()
    {
        createTower.Destroy_Tower(targetObj);
        buildRingPos.gameObject.SetActive(false);
    }
    public void Upgrade_Tower()
    {
        gameInfo.Set_Gold(-nTargetLevel * 10);
        createTower.Upgrade_Tower(targetObj);
        buildRingPos.gameObject.SetActive(false);
    }
    public void Set_Gold()
    {
        if (buildRingPos.gameObject.activeSelf)
        {
            bool _bActive = true;

            if (nTargetLevel > 3 || gameInfo.Get_Gold() < nTargetLevel * 10)
            {
                _bActive = false;
            }
            upgradeBtn.Set_Button(_bActive);
        }
        uiTopbar.Set_Gold(gameInfo.Get_Gold());
    }
    public void Set_Time()
    {
        uiTopbar.Set_Time(gameInfo.Get_Time());
    }
    public void Set_Life()
    {
        uiTopbar.Set_Life(gameInfo.Get_Life());
    }

    public void Skill_Point()
    {
        uiEventManager.Unsubscribe(sClickEvent, Skill_Point);
        messageObj.SetActive(false);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        skillManager.Create_Skill(nSkilllndex, mousePos);
    }

    public void Get_ClickObj()
    {
        if (guardRingPos.gameObject.activeSelf)
        {
            if (guardRingPos.gameObject.activeSelf == true)
                guardRingPos.gameObject.SetActive(false);

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
        }

        int layerMask = (1 << LayerMask.NameToLayer("Tower")) | (1 << LayerMask.NameToLayer("Guard"));

        Vector3 _mousePos = Input.mousePosition;
        _mousePos = Camera.main.ScreenToWorldPoint(_mousePos);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(_mousePos, transform.forward, 100f, layerMask);

        if (raycastHit2D)
        {
            Vector3 _pos = Vector3.zero;
            targetObj = raycastHit2D.collider.gameObject;

            switch (raycastHit2D.transform.tag)
            {
                case "Tower":
                    _pos = raycastHit2D.collider.gameObject.transform.position + Vector3.up * 0.4f;
                    buildRingPos.anchoredPosition = UIManager.Instance.Get_UIPos(_pos);

                    nTargetLevel = TowerManager.Instance.Get_Level(targetObj) + 1;

                    upgradeBtn.Set_Button(nTargetLevel <= 3 && gameInfo.Get_Gold() >= nTargetLevel * 10);

                    guardRingPos.gameObject.SetActive(false);
                    buildRingPos.gameObject.SetActive(true);
                    break;
                case "Guard":
                    _pos = raycastHit2D.collider.gameObject.transform.position + Vector3.up * 0.2f;
                    guardRingPos.anchoredPosition = UIManager.Instance.Get_UIPos(_pos);
                    buildRingPos.gameObject.SetActive(false);
                    guardRingPos.gameObject.SetActive(true);
                    uiEventManager.Subscribe(sClickEvent, Get_ClickMove);
                    break;
            }
        }
        else
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (buildRingPos.gameObject.activeSelf == true)
                buildRingPos.gameObject.SetActive(false);

            if (guardRingPos.gameObject.activeSelf == true)
                guardRingPos.gameObject.SetActive(false);
        }
    }
    public void Get_ClickMove()
    {
        uiEventManager.Unsubscribe(sClickEvent, Get_ClickMove);
        guardRingPos.gameObject.SetActive(false);

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = -5f;
        GuardManager.Instance.Move_Guard(targetObj, mousePos);
    }
    public void Open_PausedPopup()
    {
        gameInfo.Set_Pause(true);

        UIPaused _uiOpening = UIManager.Instance.GetUI(sUIPausedPath) as UIPaused;
        _uiOpening.Set_Action(delegate { gameInfo.Set_Pause(false); });
        _uiOpening.transform.SetAsLastSibling();
        _uiOpening.Open();
    }

    public void Update()
    {
        if (guardRingPos.gameObject.activeSelf)
        {
            Vector2 _pos = targetObj.transform.position + Vector3.up * 0.2f;
            guardRingPos.anchoredPosition = UIManager.Instance.Get_UIPos(_pos);
        }
        if (buildRingPos.gameObject.activeSelf)
        {
            Vector2 _pos = targetObj.transform.position + Vector3.up * 0.4f;
            buildRingPos.anchoredPosition = UIManager.Instance.Get_UIPos(_pos);
        }
    }
}
