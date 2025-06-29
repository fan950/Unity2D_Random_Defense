using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle_Guard : State
{
    private Guard guard;
    private IGuardManager guardManager;

    private const string sIdle = "Idle";
    public Idle_Guard(Guard guard, IGuardManager guardManager)
    {
        this.guardManager = guardManager;
        this.guard = guard;
    }
    public override void Enter()
    {
        guard.Set_AniPlay(sIdle);
    }

    public override void Update()
    {
        if (guardManager.Get_CombatMgr().FindInRange_Guard(guard) != null)
        {
            guard.Set_FSM(StateType.Attack);
        }
    }
}
