using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySystem_Bullet
{
    private ICreateTower_Obj bulletManager;
    public DestroySystem_Bullet(ICreateTower_Obj iBulletManager)
    {
        bulletManager = iBulletManager;
    }

    public void Update(List<Bullet> lisBullet)
    {
        for (int i = lisBullet.Count - 1; i >= 0; --i)
        {
            if (!lisBullet[i].Has<DestroyComponent>())
                continue;

            if (lisBullet[i].Has<ObjComponent>())
            {
                var visual = lisBullet[i].Get<ObjComponent>();
                bulletManager.Return(visual.nIndex, visual.bullet);
                visual.bullet.gameObject.SetActive(false);
                lisBullet[i].Remove<DestroyComponent>();

                lisBullet.RemoveAt(i);
            }
        }
    }
}
