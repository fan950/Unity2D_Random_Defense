using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Guard : State
{
    private Guard guard;

    private const string sIdle = "Idle";
    private const string sMove = "Move";
    public Move_Guard(Guard guard)
    {
        this.guard = guard;
    }
    public override void Enter()
    {
        guard.Set_AniPlay(sMove);
    }

    public override void Update()
    {
        guard.transform.position = Vector3.MoveTowards(guard.transform.position, guard.vecMovePoint, Time.deltaTime * guard.fSpeed);

        if (guard.transform.position.x > guard.vecMovePoint.x)
        {
            guard.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            guard.transform.rotation = Quaternion.identity;
        }

        if (Vector2.Distance(guard.transform.position, guard.vecMovePoint) <= 0.01f)
        {
            guard.Set_AniPlay(sIdle);
            guard.Set_FSM(StateType.Idle);
        }
    }
}