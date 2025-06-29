using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSystem_Bullet
{
    public void Update(List<Bullet> lisBullet, float deltaTime)
    {
        for (int i = 0; i < lisBullet.Count; ++i)
        {
            if (lisBullet[i].Has<DestroyComponent>() ||
                !lisBullet[i].Has<PositionComponent>() ||
               !lisBullet[i].Has<ObjComponent>())
                continue;

            var position = lisBullet[i].Get<PositionComponent>();
            var obj = lisBullet[i].Get<ObjComponent>();

            if (obj.bullet == null)
                continue;

            if (lisBullet[i].Has<FollowComponent>())
            {
                var target = lisBullet[i].Get<FollowComponent>();
                if (Vector3.Distance(obj.bullet.transform.position, target.target.transform.position) <= 0.1f)
                {
                    var _fValue = (target.target.transform.position - obj.bullet.transform.position).normalized;
                    lisBullet[i].Remove<FollowComponent>();
                    lisBullet[i].Add(new StraightComponent { value = _fValue });
                }
            }
            else if (lisBullet[i].Has<FixComponent>())
            {
                var target = lisBullet[i].Get<FixComponent>();
                if (Vector3.Distance(obj.bullet.transform.position, target.value) <= 0.1f)
                {
                    if (lisBullet[i].Has<FxComponent>())
                        FxManager.Instance.Get_Fx(lisBullet[i].Get<FxComponent>().parh, position.vecPos);

                    var damage = lisBullet[i].Get<DamageComponent>();
                    if (damage.damageType == DamageType.Multi)
                        CombatManager.Instance.Set_Monster_GroupDamage(obj.bullet, damage.value, damage.fSize);

                    lisBullet[i].Add(new DestroyComponent { });
                }
            }
            obj.bullet.transform.rotation = Quaternion.AngleAxis(position.fAngle, Vector3.forward);
            obj.bullet.transform.position = position.vecPos;

            if (!obj.bullet.activeSelf)
                obj.bullet.SetActive(true);

        }
    }
}
