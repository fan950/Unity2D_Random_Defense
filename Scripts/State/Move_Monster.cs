using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Monster : State
{
    private int nIndex;
    private Monster monster;
    private IMonsterManager monsterManager;
    public Move_Monster(Monster monster, IMonsterManager iMonsterManager)
    {
        this.monster = monster;
        monsterManager = iMonsterManager;
    }
    public override void Enter()
    {
        nIndex = 0;
        monster.transform.position = monsterManager.Get_Route(nIndex).transform.position;
    }

    public override void Update()
    {
        if (monsterManager.Get_Route(nIndex) == null)
            return;

        monster.transform.position = Vector2.MoveTowards(monster.transform.position, monsterManager.Get_Route(nIndex).transform.position, Time.deltaTime * monster.fSpeed);

        if (monster.transform.position.x > monsterManager.Get_Route(nIndex).transform.position.x)
            monster.spriteRenderer.transform.localRotation = Quaternion.Euler(0, 180, 0);
        else
            monster.spriteRenderer.transform.localRotation = Quaternion.identity;

        if (Vector2.Distance(monster.transform.position, monsterManager.Get_Route(nIndex).transform.position) <= 0.01f)
        {
            ++nIndex;
        }

        if (monsterManager.Get_Route(nIndex) == null)
        {
            monster.End_Move();
        }
    }
}
