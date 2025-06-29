using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Star : Skill
{
    private int nStep = 0;
    private float fScaleTime;
    public GameObject moveObj;
    public SpriteRenderer explosion;
    public override void Init(SkillData skillData, ICombatManager combatManager)
    {
        base.Init(skillData, combatManager);

        moveObj.SetActive(true);
        explosion.gameObject.SetActive(false);
        explosion.gameObject.transform.localScale = Vector3.zero;
        fScaleTime = 0;
        nStep = 0;
        bDie = false;
        fSpeed = Random.Range(10, 16);

    }
    public override void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, vecTargetPos, Time.deltaTime * fSpeed);

        Vector3 direction = vecTargetPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        moveObj.transform.rotation = Quaternion.Euler(0, 0, angle);

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
            explosion.gameObject.transform.localScale = new Vector3(fScaleTime, fScaleTime, fScaleTime);
        }

        if (fScaleTime >= 2.0f)
        {
            bDie = true;
        }
    }
    public void Explosion()
    {
        moveObj.SetActive(false);
        explosion.gameObject.SetActive(true);

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

