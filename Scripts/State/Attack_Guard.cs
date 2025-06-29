using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Guard : State
{
    private Guard guard;
    private float fSpeedTime;
    private IGuardManager guardManager;

    private const string sIdle= "Idle";
    private const string sAttack = "Attack";
    public Attack_Guard(Guard guard, IGuardManager guardManager)
    {
        this.guardManager = guardManager;
        this.guard = guard;
    }
    public override void Enter()
    {
        fSpeedTime = guard.fAttackSpeed;
    }

    public override void Exit()
    {
        guard.Set_AniPlay(sIdle);
    }

    public override void Update()
    {
        if (guardManager.Get_CombatMgr().FindInRange_Guard(guard)==null) 
        {
            guard.Set_FSM(StateType.Idle);
            return;
        }

        fSpeedTime += Time.deltaTime;
        if (fSpeedTime >= guard.fAttackSpeed)
        {
            fSpeedTime = 0;
            guard.Set_AniPlay(sAttack);
        }
    }
}
