using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Tower : State
{
    private Tower tower;
    private GameObject characterObj;
    private float fSpeed;

    private const string sAttack = "Attack";

    public Attack_Tower(Tower tower,GameObject characterObj)
    {
        this.tower = tower;
        this.characterObj = characterObj;
    }

    public override void Update()
    {
        fSpeed += Time.deltaTime;
        if (fSpeed>=tower.fMaxSpeed) 
        {
            tower.Set_AniPlay(sAttack);
            fSpeed = 0;
        }

        if (characterObj != null)
        {
            if (tower.transform.position.x > tower.targetObj.transform.position.x)
                characterObj.transform.rotation = Quaternion.Euler(0, 180, 0);
            else
                characterObj.transform.rotation = Quaternion.identity;
        }
    }

}
