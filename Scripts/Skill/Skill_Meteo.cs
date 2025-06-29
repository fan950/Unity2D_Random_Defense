using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Meteo : Skill
{
    private int nStep = 0;
    private float fScaleTime;
    public GameObject moveObj;
    public GameObject explosionObj;
    public override void Init(SkillData skillData, ICombatManager combatManager)
    {
        base.Init(skillData, combatManager);

        moveObj.SetActive(true);
        explosionObj.SetActive(false);
        explosionObj.transform.localScale = Vector3.zero;
        fScaleTime = 0;
        nStep = 0;
        bDie = false;
        fSpeed = 15;
    }
    public override void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, vecTargetPos, Time.deltaTime * fSpeed);

        Vector3 _direction = vecTargetPos - transform.position;
        float _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg + 90;
        moveObj.transform.rotation = Quaternion.Euler(0, 0, _angle);

        if (Vector2.Distance(transform.position, vecTargetPos) <= 0.01f)
        {
            nStep = 1;
        }
    }
    public void ScaleUp()
    {
        fScaleTime += Time.deltaTime * 2;
        if (fScaleTime <= 1)
        {
            explosionObj.transform.localScale = new Vector3(fScaleTime, fScaleTime, fScaleTime);
        }

        if (fScaleTime >= 2.0f)
        {
            bDie = true;
        }
    }
    public void Explosion()
    {
        moveObj.SetActive(false);
        explosionObj.SetActive(true);

        combatManager.Set_Monster_GroupDamage(gameObject, skillData.nDamage, skillData.fDamageSize);
        nStep = 2;
    }
    public override void Update_Skill()
    {
        switch (nStep)
        {
            case 0:
                Move();
                break;
            case 1:
                Explosion();
                break;
            case 2:
                ScaleUp();
                break;

        }
    }
}
