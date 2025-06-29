using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct State_Guard
{
    public int nAttack;
    public float fMaxSpeed;
    public float fAttackRange;
    public float fAttackSpeed;
    public void Set_Stat(int nAttack, float fMaxSpeed, float fAttackRange, float fAttackSpeed)
    {
        this.nAttack = nAttack;
        this.fMaxSpeed = fMaxSpeed;
        this.fAttackRange = fAttackRange;
        this.fAttackSpeed = fAttackSpeed;
    }
}
public class Guard : MonoBehaviour
{
    public int nIndex;

    protected Animator animator;
    protected State_Guard state;
    protected DamageType damageType;
    protected StateType stateType;
    public StateType Get_State { get { return stateType; } }
    protected Dictionary<StateType, State> dicState;
    [HideInInspector] public Vector3 vecMovePoint;
    [HideInInspector] public float fSpeed = 1;
    protected IGuardManager guardManager;
    public int nAttack { get { return state.nAttack; } }
    public float fAttackSpeed { get { return state.fAttackSpeed; } }
    public float fAttackRange { get { return state.fAttackRange; } }

    private Action<int, GameObject> action;
    public Action<int, GameObject> action_Die { set { action = value; } }

    public virtual void Init(int nAttack, GuardData data, DamageType damageType, IGuardManager guardManager)
    {
        stateType = StateType.Idle;

        if (dicState != null)
            return;

        this.damageType = damageType;
        nIndex = data.nIndex;
        state.Set_Stat(nAttack, data.fMoveSpeed, data.fAttackRange, data.fAttackSpeed);

        animator = GetComponent<Animator>();
        this.guardManager = guardManager;
        dicState = new Dictionary<StateType, State>();
        dicState.Add(StateType.Idle, new Idle_Guard(this, guardManager));
        dicState.Add(StateType.Attack, new Attack_Guard(this, guardManager));
        dicState.Add(StateType.Move, new Move_Guard(this));
        dicState.Add(StateType.Die, new Die_Guard(this));
    }
    public virtual void Update_Guard()
    {
        dicState[stateType].Update();
    }

    public void Set_FSM(StateType stateType)
    {
        if (this.stateType == stateType)
            return;

        dicState[this.stateType].Exit();
        dicState[stateType].Enter();
        this.stateType = stateType;
    }
    public void Set_Move(Vector3 pos)
    {
        vecMovePoint = pos;
        Set_FSM(StateType.Move);
    }

    public void Set_AniPlay(string sName)
    {
        if (animator == null)
            return;

        animator.SetTrigger(sName);
    }
    public virtual void Apply_Attack()
    {
        GameObject _obj = guardManager.Get_CombatMgr().Combat_Guard(this);
        if (_obj != null)
        {
            if (_obj.transform.position.x < transform.position.x)
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            else
                transform.localRotation = Quaternion.identity;
        }
    }
    public void Die()
    {
        action(nIndex, gameObject);
    }
}
