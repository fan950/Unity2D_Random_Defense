using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSystem_Bullet
{
    public void Update(List<Bullet> lisBullet, float deltaTime)
    {
        for (int i = 0; i < lisBullet.Count; ++i)
        {
            if (lisBullet[i].Has<DestroyComponent>() ||
                !lisBullet[i].Has<PositionComponent>() ||
                  !lisBullet[i].Has<SpeedComponent>())
                continue;

            var position = lisBullet[i].Get<PositionComponent>();
            var speed = lisBullet[i].Get<SpeedComponent>();

            if (lisBullet[i].Has<FollowComponent>())
            {
                var target = lisBullet[i].Get<FollowComponent>();
                Vector3 vecTarget = (target.target.position - position.vecPos).normalized;
                position.fAngle = Mathf.Atan2(vecTarget.y, vecTarget.x) * Mathf.Rad2Deg;
                position.vecPos += vecTarget.normalized * speed.fSpeed * deltaTime;
            }
            else if (lisBullet[i].Has<StraightComponent>())
            {
                var direction = lisBullet[i].Get<StraightComponent>();
                position.vecPos += direction.value * speed.fSpeed * deltaTime;
                position.fAngle = Mathf.Atan2(direction.value.y, direction.value.x) * Mathf.Rad2Deg;
            }
            else if (lisBullet[i].Has<FixComponent>())
            {
                var fix = lisBullet[i].Get<FixComponent>();
                Vector3 vecTarget = (fix.value - position.vecPos).normalized;
                position.fAngle = Mathf.Atan2(vecTarget.y, vecTarget.x) * Mathf.Rad2Deg;
                position.vecPos += vecTarget.normalized * speed.fSpeed * deltaTime;
            }

            lisBullet[i].Set(position);
        }
    }
}
