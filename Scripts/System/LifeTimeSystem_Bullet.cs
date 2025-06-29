using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimeSystem_Bullet
{
    private const float fMaxTime = 20;
    public void Update(List<Bullet> lisBullet)
    {
        for (int i = 0; i < lisBullet.Count; ++i)
        {
            if (lisBullet[i].Has<DestroyComponent>() ||
                !lisBullet[i].Has<LifeTimeComponent>())
                continue;

            var life = lisBullet[i].Get<LifeTimeComponent>();

            life.fLifeTime += Time.deltaTime;

            lisBullet[i].Set(life);

            if (life.fLifeTime>= fMaxTime) 
            {
                lisBullet[i].Add(new DestroyComponent { });
            }
        }
    }
}
