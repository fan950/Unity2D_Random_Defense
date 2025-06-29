using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die_Guard : State
{
    private Guard guard;

    private const string sDie = "Die";
    public Die_Guard(Guard guard)
    {
        this.guard = guard;
    }
    public override void Enter()
    {
        guard.Set_AniPlay(sDie);
    }
}
