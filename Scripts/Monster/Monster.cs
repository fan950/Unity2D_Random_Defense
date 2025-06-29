using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct State_Monster
{
    public int nMaxHp;
    public int nHp;
    public float fMoveSpeed;
    public void Set_Stat(int nHp, float fMoveSpeed)
    {
        nMaxHp = nHp;
        this.nHp = nHp;
        this.fMoveSpeed = fMoveSpeed;
    }
}
public enum StateType
{
    Idle,
    Move,
    Attack,
    Die
}
public abstract class Monster : MonoBehaviour
{
    [HideInInspector] public bool bDie;
    [HideInInspector] public int nIndex;
    protected Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public GameObject pivotObj;
    protected State_Monster state;
    protected StateType stateType;
    public StateType Get_State { get { return stateType; } }
    protected Dictionary<StateType, State> dicState;
    private IMonsterManager monsterManager;

    [HideInInspector] public GameObject HeadObj;
    public int nMaxHp { get { return state.nMaxHp; } }
    public int nHp { get { return state.nHp; } set { state.nHp = value; } }
    public float fSpeed { get { return state.fMoveSpeed; } }
    public virtual void Init(int nIndex, IMonsterManager iMonsterManager)
    {
        this.nIndex = nIndex;
        bDie = false;

        MonsterData _data = iMonsterManager.Get_Data(nIndex);
        state.Set_Stat(_data.nHp, _data.fSpeed);

        if (HeadObj == null)
            HeadObj = transform.Find("Head").gameObject;
        if (pivotObj == null)
            pivotObj = transform.Find("Pivot").gameObject;

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (animator == null)
            animator = GetComponent<Animator>();

        spriteRenderer.color = new Color(1, 1, 1, 1);
        monsterManager = iMonsterManager;
    }
    public virtual void Update_Monster()
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
    public void Die()
    {
        bDie = true;
    }
    public void Set_AniPlay(string sName)
    {
        animator.SetTrigger(sName);
    }
    public void End_Move()
    {
        monsterManager.End_Move();
        Set_FSM(StateType.Die);
    }
}
