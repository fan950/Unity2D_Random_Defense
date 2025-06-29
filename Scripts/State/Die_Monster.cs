using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die_Monster : State
{
    private Monster monster;
    private float fColorTime;
    private bool bDieAni;

    private const string sDie = "Die";

    public Die_Monster(Monster monster, bool bDieAni)
    {
        this.monster = monster;
        this.bDieAni = bDieAni;
    }
    public override void Enter()
    {
        fColorTime = 1;
        if (bDieAni)
        {
            monster.Set_AniPlay(sDie);
        }
    }

    public override void Update()
    {
        if (!bDieAni)
        {
            fColorTime -= Time.deltaTime;
            monster.spriteRenderer.color = new Color(1, 1, 1, fColorTime);

            if (fColorTime <= 0)
            {
                monster.Die();
            }
        }
    }
}
