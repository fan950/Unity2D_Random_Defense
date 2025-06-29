using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHUD : UIBase
{
    public GameObject uiHpbarBox;
    public GameObject uiDamageBox;

    private ObjcetPool<UIHpbar> uiHpbar_Pool = new ObjcetPool<UIHpbar>();
    private ObjcetPool<UIDamage> uiDamage_Pool = new ObjcetPool<UIDamage>();

    private List<UIHpbar> lisActive_Hpbar = new List<UIHpbar>();
    private List<UIDamage> lisActive_Damage = new List<UIDamage>();

    Dictionary<GameObject, Action<GameObject, int, int>> dicTempHpbar = new Dictionary<GameObject, Action<GameObject, int, int>>();
    Dictionary<GameObject, Action<GameObject, int>> dicTempDamage = new Dictionary<GameObject, Action<GameObject, int>>();

    private ICombatManager combatManager;

    private const string sHpbarPath = "UI/UIHpbar";
    private const string sDamagePath = "UI/UIDamage";
    private const string sMonsterEvent = "Monster";
    public override void Init(IUIEventManager uIButtonEventManager)
    {
        int _nCount = 10;
        base.Init(uiEventManager);
        uiHpbar_Pool.Init(sHpbarPath, _nCount, uiHpbarBox.transform);
        uiDamage_Pool.Init(sDamagePath, _nCount, uiDamageBox.transform);
    }
    public void Set_CombatMgr(ICombatManager combatManager)
    {
        this.combatManager = combatManager;
    }
    public override void Open()
    {
        combatManager.Subscribe(sMonsterEvent, Set_Combat);
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        combatManager.Unsubscribe(sMonsterEvent, Set_Combat);
        gameObject.SetActive(false);
    }

    public void Set_Combat(GameObject target, int nMaxHp, int nHp, int nDamage)
    {
        if (nHp <= 0)
        {
            Die_Obj(target);
            return;
        }

        if (dicTempHpbar.ContainsKey(target))
        {
            dicTempHpbar[target](target, nMaxHp, nHp);
            dicTempDamage[target](target, nDamage);
        }
        else
        {
            UIHpbar _uiHpbar = uiHpbar_Pool.Get();
            UIDamage _uiDamage = uiDamage_Pool.Get();

            _uiHpbar.Init();
            _uiDamage.Init();

            _uiHpbar.Set_Hpbar(target, nMaxHp, nHp);
            _uiDamage.Set_Damage(target, nDamage);

            lisActive_Hpbar.Add(_uiHpbar);
            lisActive_Damage.Add(_uiDamage);

            dicTempHpbar.Add(target, _uiHpbar.Set_Hpbar);
            dicTempDamage.Add(target, _uiDamage.Set_Damage);
        }
    }
    public void Die_Obj(GameObject target)
    {
        if (dicTempHpbar.ContainsKey(target))
        {
            for (int i = lisActive_Hpbar.Count - 1; i >= 0; --i)
            {
                if (lisActive_Hpbar[i].targetObj == target)
                {
                    lisActive_Hpbar[i].bActive = false;
                    break;
                }
            }

            for (int i = lisActive_Damage.Count - 1; i >= 0; --i)
            {
                if (lisActive_Damage[i].targetObj == target)
                {
                    lisActive_Damage[i].bActive = false;
                    break;
                }
            }
        }
    }
    public void Update()
    {
        if (lisActive_Hpbar.Count > 0)
        {
            for (int i = lisActive_Hpbar.Count - 1; i >= 0; --i)
            {
                lisActive_Hpbar[i].Update_UI();
                if (!lisActive_Hpbar[i].bActive)
                {
                    dicTempHpbar.Remove(lisActive_Hpbar[i].targetObj);
                    dicTempDamage.Remove(lisActive_Hpbar[i].targetObj);

                    uiHpbar_Pool.Return(lisActive_Hpbar[i]);
                    lisActive_Hpbar.RemoveAt(i);
                }
            }
        }
        if (lisActive_Damage.Count > 0)
        {
            for (int i = lisActive_Damage.Count - 1; i >= 0; --i)
            {
                lisActive_Damage[i].Update_UI();
                if (!lisActive_Damage[i].bActive)
                {
                    uiDamage_Pool.Return(lisActive_Damage[i]);
                    lisActive_Damage.RemoveAt(i);
                }
            }
        }
    }
}
