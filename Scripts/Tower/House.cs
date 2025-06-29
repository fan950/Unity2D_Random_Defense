using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Tower
{
    private const int nCountMax = 3;
    private List<Guard> lisGuard = new List<Guard>();

    private const string sAttack = "Attack";
    public override void Init(TowerData data, ICreateTower_Obj iTowerCreate)
    {
        lisGuard.Clear();
        base.Init(data, iTowerCreate);
    }
    public override void Update_Tower()
    {
        if (bAttack == false|| lisGuard.Count >= 3)
            return;

        fSpeedTime += Time.deltaTime;
        if (fSpeedTime >= fMaxSpeed)
        {
            Set_AniPlay(sAttack);
            fSpeedTime = 0;
        }

    }
    public override void Create_Obj()
    {
        if (lisGuard.Count >= nCountMax)
            return;

        lisGuard.Add(iTowerCreate.Create_Obj(nAttack, nBulletIndex, new Vector3(launcherObj.transform.position.x, launcherObj.transform.position.y, -5), DamageType.Single, null, 1).GetComponent<Guard>());
    }

    public override void Die()
    {
        for (int i = 0; i < lisGuard.Count; ++i)
        {
            lisGuard[i].Set_FSM(StateType.Die);
        }
    }
}
