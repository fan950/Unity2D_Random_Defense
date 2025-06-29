using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct State_Tower
{
    public int nLevel;
    public int nAttack;
    public float fMaxSpeed;
    public float fAttackRange;
    public CreateType createType;
    public DamageType damageType;
    public int nBulletIndex;
    public void Set_Stat(int nLevel, int nAttack, float fAttackSpeed, float fAttackRange, CreateType createType, DamageType damageType, int nBulletIndex)
    {
        this.nLevel = nLevel;
        this.nAttack = nAttack;
        this.fMaxSpeed = fAttackSpeed;
        this.fAttackRange = fAttackRange;
        this.createType = createType;
        this.damageType = damageType;
        this.nBulletIndex = nBulletIndex;
    }
}
public abstract class Tower : MonoBehaviour
{
    public int nIndex;
    public bool bAttack;
    protected State_Tower state;
    protected Animator animator;
    [HideInInspector] public GameObject targetObj;
    public GameObject launcherObj;
    protected ICreateTower_Obj iTowerCreate;
    protected DamageType damageType;
    protected float fSpeedTime;
    public int nLevel { get { return state.nLevel; } }
    public int nAttack { get { return state.nAttack; } }
    public int nBulletIndex { get { return state.nBulletIndex; } }
    public float fAttackRange { get { return state.fAttackRange; } }
    public float fMaxSpeed { get { return state.fMaxSpeed; } }
    public virtual void Init(TowerData data, ICreateTower_Obj iTowerCreate)
    {
        nIndex = data.nIndex;
        animator = GetComponent<Animator>();
        this.iTowerCreate = iTowerCreate;

        state.Set_Stat(data.nLevel, data.nAttack, data.fAttackSpeed, data.fAttackRange, data.createType,data.damageType,data.nBulletIndex);
        fSpeedTime = data.fAttackSpeed;
    }
    public abstract void Update_Tower();
    public abstract void Create_Obj();
    public virtual void Die() { }
    public void Set_Attack(GameObject obj)
    {
        bAttack = true;
        targetObj = obj;
    }
    public void Set_AniPlay(string sName)
    {
        animator.SetTrigger(sName);
    }
}
