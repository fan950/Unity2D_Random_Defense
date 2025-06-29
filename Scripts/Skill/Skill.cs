using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [HideInInspector] public bool bDie;
    protected SkillData skillData;
    public int nIndex { get { return skillData.nIndex; } }
    protected Vector3 vecTargetPos;
    protected ICombatManager combatManager;
    protected float fSpeed;
    public virtual void Init(SkillData skillData, ICombatManager combatManager)
    {
        bDie = false;
        this.skillData = skillData;
        this.combatManager = combatManager;
    }
    public void Set_Skill(Vector3 vecPos)
    {
        vecTargetPos = vecPos;
    }
    public abstract void Move();
    public abstract void Update_Skill();
}
