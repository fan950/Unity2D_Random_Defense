using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICreateTower_Obj
{
    public GameObject Create_Obj(int nAttack, int nIndex, Vector3 position, DamageType damageType, Transform target, float fAttackSize);
    public void Return(int nIndex, GameObject obj);
}
public interface IGuardManager
{
    public ICombatManager Get_CombatMgr();
    public ICreateTower_Obj Get_BulletMgr();
    public void Move_Guard(GameObject obj, Vector3 pos);
}
public interface ICombatGuard
{
    public List<Guard> Get_lisGuard();
}
public class GuardManager : Singleton<GuardManager>, IGuardManager, ICreateTower_Obj, ICombatGuard
{
    private GuardDataTable guardDataTable;
    private Dictionary<int, ObjcetPool<Guard>> dicGuard = new Dictionary<int, ObjcetPool<Guard>>();

    private List<Guard> lisActiveGuard = new List<Guard>();
    private List<GameObject> lisDestroyGuard = new List<GameObject>();

    private const string sTablePath = "Table/GuardTable";
    public void Set_Guard()
    {
        guardDataTable = Resources.Load(sTablePath) as GuardDataTable;
        guardDataTable.Init();

        for (int i = 0; i < guardDataTable.lisGuardData.Count; ++i)
        {
            if (!dicGuard.ContainsKey(guardDataTable.lisGuardData[i].nIndex))
                dicGuard.Add(guardDataTable.lisGuardData[i].nIndex, new ObjcetPool<Guard>());
            dicGuard[guardDataTable.lisGuardData[i].nIndex].Init(guardDataTable.lisGuardData[i].obj, 5, transform);
        }
    }
    public void Move_Guard(GameObject obj, Vector3 pos)
    {
        for (int i = 0; i < lisActiveGuard.Count; ++i)
        {
            if (lisActiveGuard[i].gameObject == obj)
            {
                lisActiveGuard[i].Set_Move(pos);
            }
        }
    }
    public void Update_Mgr()
    {
        for (int i = 0; i < lisActiveGuard.Count; ++i)
        {
            lisActiveGuard[i].Update_Guard();
        }

        if (lisDestroyGuard.Count > 0)
        {
            for (int i = 0; i < lisDestroyGuard.Count; ++i)
            {
                for (int j = 0; j < lisActiveGuard.Count; ++j)
                {
                    if (lisDestroyGuard[i] == lisActiveGuard[j].gameObject)
                    {
                        dicGuard[lisActiveGuard[j].nIndex].Return(lisActiveGuard[j]);
                        lisActiveGuard.RemoveAt(j);
                        break;
                    }
                }
            }
            lisDestroyGuard.Clear();
        }
    }

    public GameObject Create_Obj(int nAttack, int nIndex, Vector3 position, DamageType damageType, Transform target, float fAttackSize)
    {
        Guard _guard = dicGuard[nIndex].Get();
        _guard.Init(nAttack, guardDataTable.Get_Data(nIndex), damageType, this);
        _guard.action_Die = Return;
        _guard.transform.position = position;
        lisActiveGuard.Add(_guard);

        return _guard.gameObject;
    }

    public void Return(int nIndex, GameObject obj)
    {
        lisDestroyGuard.Add(obj);
    }

    public List<Guard> Get_lisGuard()
    {
        return lisActiveGuard;
    }

    public ICombatManager Get_CombatMgr()
    {
        return CombatManager.Instance;
    }

    public ICreateTower_Obj Get_BulletMgr()
    {
        return BulletManager.Instance;
    }
}
