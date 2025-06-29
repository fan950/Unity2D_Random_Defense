using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician : Tower
{
    private GameObject characterObj;

    private const string sCharacter = "Character";
    private const string sAttack = "Attack";
    public override void Init(TowerData data, ICreateTower_Obj iTowerCreate)
    {
        base.Init(data, iTowerCreate);
        characterObj = transform.Find(sCharacter).gameObject;
    }
    public override void Update_Tower()
    {
        if (bAttack == false)
            return;

        fSpeedTime += Time.deltaTime;
        if (fSpeedTime >= fMaxSpeed)
        {
            Set_AniPlay(sAttack);
            fSpeedTime = 0;
        }

        if (characterObj != null)
        {
            if (transform.position.x > targetObj.transform.position.x)
                characterObj.transform.rotation = Quaternion.Euler(0, 180, 0);
            else
                characterObj.transform.rotation = Quaternion.identity;
        }
    }

    public override void Create_Obj()
    {
        iTowerCreate.Create_Obj(nAttack,nBulletIndex, launcherObj.transform.position, state.damageType, targetObj.transform, 1);
    }
}
