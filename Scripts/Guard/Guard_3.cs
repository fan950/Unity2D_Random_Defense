using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard_3 : Guard
{
    public GameObject launcherObj;
    private const int nBulletIndex = 5;
    public override void Apply_Attack()
    {
        Monster monster = guardManager.Get_CombatMgr().FindInRange_Guard(this);
        if (monster != null)
        {
            if (monster.transform.position.x < transform.position.x)
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            else
                transform.localRotation = Quaternion.identity;

            guardManager.Get_BulletMgr().Create_Obj(nAttack, nBulletIndex, launcherObj.transform.position, damageType, monster.transform, 1);
        }
    }
}
