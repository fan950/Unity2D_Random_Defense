using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatManager
{
    public void Set_Monster_GroupDamage(GameObject obj, int nDamage, float fSize);
    public Monster FindInRange_Guard(Guard guard);
    public GameObject Combat_Guard(Guard guard);

    public void Subscribe(string eventId, Action<GameObject, int, int, int> action);
    public void Unsubscribe(string eventId, Action<GameObject, int, int, int> action);
    public void Trigger(string eventId, GameObject target, int nMaxHp, int nHp, int nDamage);
}
public class CombatManager : Singleton<CombatManager>, ICombatManager
{
    private Dictionary<string, Action<GameObject, int, int, int>> dicCombat = new Dictionary<string, Action<GameObject, int, int, int>>();
    public Action<int> dieMonsterAction;

    private ITowerManager towerManager;
    private IMonsterManager monsterManager;
    private IBulletManager bulletManager;

    private const string sMonsterEvent = "Monster";
    public void Set_Combat(ITowerManager towerManager, IMonsterManager monsterManager, IBulletManager bulletManager)
    {
        this.towerManager = towerManager;
        this.monsterManager = monsterManager;
        this.bulletManager = bulletManager;
    }
    public void Update_Mgr()
    {
        for (int i = 0; i < towerManager.Get_lisTower().Count; ++i)
        {
            Tower _tower = towerManager.Get_lisTower()[i];
            Monster _monster = FindInRange_Player(_tower);
            if (_monster != null)
            {
                _tower.Set_Attack(_monster.pivotObj);
            }
            else
            {
                _tower.bAttack = false;
            }
        }

        for (int i = 0; i < bulletManager.Get_lisBullet().Count; ++i)
        {
            Bullet bullet = bulletManager.Get_lisBullet()[i];
            Monster _monster = FindInRange_Bullet(bullet);
            if (_monster != null)
            {
                if (bullet.Has<FxComponent>())
                    FxManager.Instance.Get_Fx(bullet.Get<FxComponent>().parh, _monster.transform.position);

                switch (bullet.Get<DamageComponent>().damageType)
                {
                    case DamageType.Single:
                        Set_Monster_Damager(_monster, bullet.Get<DamageComponent>().value);
                        break;
                    case DamageType.Multi:
                        var damage = bullet.Get<DamageComponent>();
                        Set_Monster_GroupDamage(_monster.gameObject, damage.value, damage.fSize);
                        break;
                }
                bullet.Add(new DestroyComponent { });
            }
        }
    }
    public Monster FindInRange_Player(Tower tower)
    {
        var _lisMonster = monsterManager.Get_lisMonster();
        for (int i = 0; i < _lisMonster.Count; ++i)
        {
            if (_lisMonster[i].Get_State == StateType.Die)
            {
                continue;
            }

            float sqrDist = (tower.transform.position - _lisMonster[i].pivotObj.transform.position).sqrMagnitude;
            if (sqrDist <= tower.fAttackRange * tower.fAttackRange)
            {
                return _lisMonster[i];
            }
        }

        return null;
    }
    public Monster FindInRange_Guard(Guard guard)
    {
        var _lisMonster = monsterManager.Get_lisMonster();
        for (int i = 0; i < _lisMonster.Count; ++i)
        {
            if (_lisMonster[i].Get_State == StateType.Die)
            {
                continue;
            }

            float sqrDist = ((Vector2)guard.transform.position - (Vector2)_lisMonster[i].pivotObj.transform.position).sqrMagnitude;
            if (sqrDist <= guard.fAttackRange * guard.fAttackRange)
            {
                return _lisMonster[i];
            }
        }

        return null;
    }
    public GameObject Combat_Guard(Guard guard)
    {
        Monster _monster = FindInRange_Guard(guard);
        if (_monster != null)
        {
            Set_Monster_Damager(_monster, guard.nAttack);
            return _monster.gameObject;
        }
        return null;
    }

    public Monster FindInRange_Bullet(Bullet bullet)
    {
        var _lisMonster = monsterManager.Get_lisMonster();

        if (!bullet.Has<ColliderComponent>() ||
                       !bullet.Has<ObjComponent>())
            return null;

        var collider = bullet.Get<ColliderComponent>();
        var obj = bullet.Get<ObjComponent>();

        for (int i = 0; i < _lisMonster.Count; ++i)
        {
            if (_lisMonster[i].Get_State == StateType.Die)
            {
                continue;
            }

            if (Vector2.Distance(obj.bullet.transform.position, _lisMonster[i].pivotObj.transform.position) <= collider.size)
            {
                return _lisMonster[i];
            }
        }

        return null;
    }

    public void Set_Monster_GroupDamage(GameObject obj, int nDamage, float fSize)
    {
        var _lisMonster = monsterManager.Get_lisMonster();

        for (int i = 0; i < _lisMonster.Count; ++i)
        {
            if (_lisMonster[i].Get_State == StateType.Die)
            {
                continue;
            }

            if (Vector2.Distance(obj.transform.position, _lisMonster[i].pivotObj.transform.position) <= fSize)
            {
                Set_Monster_Damager(_lisMonster[i], nDamage);
            }
        }
    }
    public void Set_Monster_Damager(Monster monster, int nDamage)
    {
        monster.nHp -= nDamage;
        Trigger(sMonsterEvent, monster.HeadObj, monster.nMaxHp, monster.nHp, nDamage);
        if (monster.nHp <= 0)
        {
            monster.Set_FSM(StateType.Die);
            dieMonsterAction(UnityEngine.Random.Range(3,7));
        }
    }
    public void Subscribe(string eventId, Action<GameObject, int, int, int> action)
    {
        if (!dicCombat.ContainsKey(eventId))
        {
            dicCombat.Add(eventId, null);
        }

        dicCombat[eventId] += action;
    }

    public void Unsubscribe(string eventId, Action<GameObject, int, int, int> action)
    {
        if (dicCombat.ContainsKey(eventId))
            dicCombat[eventId] -= action;
    }

    public void Trigger(string eventId, GameObject target, int nMaxHp, int nHp, int nDamage)
    {
        if (dicCombat.ContainsKey(eventId))
            dicCombat[eventId](target, nMaxHp, nHp, nDamage);
    }
}
