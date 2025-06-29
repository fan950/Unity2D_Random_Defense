using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Straight,
    Follow,
    Fix,
}
public interface IBulletManager
{
    public List<Bullet> Get_lisBullet();
}
//ECS ±¸Á¶
// Entity => Bullet
// Component => Component
// System => BulletManager
public class BulletManager : Singleton<BulletManager>, IBulletManager, ICreateTower_Obj
{
    private BulletDataTable bulletDataTable;

    private Dictionary<GameObject, Bullet> dicSaveBullet = new Dictionary<GameObject, Bullet>();
    private Dictionary<int, Stack<Bullet>> dicBullet = new Dictionary<int, Stack<Bullet>>();
    private List<Bullet> lisActiveBullet = new List<Bullet>();

    private MoveSystem_Bullet moveSystem;
    private ObjSystem_Bullet objSystem;
    private DestroySystem_Bullet destroySystem;
    private LifeTimeSystem_Bullet lifeTimeSystem;

    private const string sTablePath = "Table/BulletTable";

    public override void Awake()
    {
        base.Awake();

        bulletDataTable = Resources.Load(sTablePath) as BulletDataTable;
        bulletDataTable.Init();

        moveSystem = new MoveSystem_Bullet();
        objSystem = new ObjSystem_Bullet();
        lifeTimeSystem = new LifeTimeSystem_Bullet();
        destroySystem = new DestroySystem_Bullet(this);
    }
    public void Create_Pool(BulletData bulletData, int nAttack, Vector3 position, DamageType damageType, Transform target, float fAttackSize)
    {
        Stack<Bullet> _temp = new Stack<Bullet>();
        for (int i = 0; i < 10; ++i)
        {
            var bullet = new Bullet();
            bullet.Add(new PositionComponent { vecPos = position, fAngle = 0 });
            bullet.Add(new SpeedComponent { fSpeed = bulletData.fSpeed });
            bullet.Add(new ColliderComponent { size = 0.3f });
            bullet.Add(new LifeTimeComponent { fLifeTime = 0 });
            bullet.Add(new DamageComponent { damageType = damageType, value = nAttack, fSize = fAttackSize }); ;
            bullet.Add(new ObjComponent
            {
                nIndex = bulletData.nIndex,
                bullet = Instantiate(bulletData.obj, transform)
            });

            if (bulletData.sFxPath != string.Empty)
                bullet.Add(new FxComponent { parh = bulletData.sFxPath });

            switch (bulletData.bulletType)
            {
                case BulletType.Straight:
                    bullet.Add(new StraightComponent { value = (target.position - position).normalized });
                    break;
                case BulletType.Follow:
                    bullet.Add(new FollowComponent { target = target });
                    break;
                case BulletType.Fix:
                    bullet.Add(new FixComponent { value = target.transform.position });
                    break;
            }

            var getObj = bullet.Get<ObjComponent>();
            getObj.bullet.SetActive(false);
            bullet.Set(getObj);

            _temp.Push(bullet);

            bullet.nIndex = i;
        }
        if (!dicBullet.ContainsKey(bulletData.nIndex))
            dicBullet.Add(bulletData.nIndex, _temp);
        else
            dicBullet[bulletData.nIndex] = _temp;
    }
    public GameObject Create_Obj(int nAttack, int nIndex, Vector3 position, DamageType damageType, Transform target, float fAttackSize)
    {
        BulletData bulletData = bulletDataTable.Get_Bullet(nIndex);

        if (!dicBullet.ContainsKey(nIndex) || dicBullet[nIndex].Count <= 0)
        {
            Create_Pool(bulletData, nAttack, position, damageType, target, fAttackSize);
        }

        var bullet = dicBullet[nIndex].Pop();

        var getPos = bullet.Get<PositionComponent>();
        getPos.vecPos = position;
        getPos.fAngle = 0;
        bullet.Set(getPos);

        var getDamage = bullet.Get<DamageComponent>();
        getDamage.value = nAttack;
        bullet.Set(getDamage);

        switch (bulletData.bulletType)
        {
            case BulletType.Straight:
                if (bullet.Has<StraightComponent>())
                {
                    var getStraight = bullet.Get<StraightComponent>();
                    getStraight.value = (target.position - position).normalized;
                    bullet.Set(getStraight);
                }
                break;
            case BulletType.Follow:
                if (bullet.Has<FollowComponent>())
                {
                    var getFollow = bullet.Get<FollowComponent>();
                    getFollow.target = target;
                    bullet.Set(getFollow);
                }
                else
                {
                    if (bullet.Has<StraightComponent>())
                    {
                        bullet.Remove<StraightComponent>();
                    }
                    bullet.Add(new FollowComponent { target = target });
                }
                break;
            case BulletType.Fix:
                if (bullet.Has<FixComponent>())
                {
                    var getFix = bullet.Get<FixComponent>();
                    getFix.value = target.transform.position;
                    bullet.Set(getFix);
                }
                break;
        }

        var life = bullet.Get<LifeTimeComponent>();
        life.fLifeTime = 0;
        bullet.Set(life);

        var getObj = bullet.Get<ObjComponent>();
        getObj.bullet.transform.rotation = Quaternion.identity;
        getObj.bullet.transform.position = getPos.vecPos;
        bullet.Set(getObj);

        lisActiveBullet.Add(bullet);

        if (!dicSaveBullet.ContainsKey(getObj.bullet))
        {
            dicSaveBullet.Add(getObj.bullet, bullet);
        }

        return getObj.bullet;
    }
    public void Update_Mgr()
    {
        float dt = Time.deltaTime;

        moveSystem.Update(lisActiveBullet, dt);
        objSystem.Update(lisActiveBullet, dt);
        lifeTimeSystem.Update(lisActiveBullet);
        destroySystem.Update(lisActiveBullet);
    }
    public List<Bullet> Get_lisBullet()
    {
        return lisActiveBullet;
    }

    public void Return(int nIndex, GameObject obj)
    {
        dicBullet[nIndex].Push(dicSaveBullet[obj]);
    }

}
