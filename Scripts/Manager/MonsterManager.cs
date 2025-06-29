using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMonsterManager
{
    public void End_Move();
    public MonsterData Get_Data(int nIndex);
    public GameObject Get_Route(int nIndex);
    public List<Monster> Get_lisMonster();
}
public class MonsterManager : Singleton<MonsterManager>, IMonsterManager
{
    private IGameInfo gameInfo;
    private MonsterDataTable monsterTable;
    private Dictionary<int, ObjcetPool<Monster>> dicMonster;
    private GameObject[] arrRouteObj;

    private List<Monster> lisActiveMonster = new List<Monster>();

    private const string sTablePath = "Table/MonsterTable"; 
    public void Set_Wave(IGameInfo gameInfo, GameObject[] arrRouteObj, WaveData waveData)
    {
        this.gameInfo = gameInfo;
        monsterTable = Resources.Load(sTablePath) as MonsterDataTable;
        monsterTable.Init();

        int _nCount = 10;
        dicMonster = new Dictionary<int, ObjcetPool<Monster>>();
        for (int i = 0; i < waveData.nArrMonsterIndex.Length; ++i)
        {
            dicMonster.Add(waveData.nArrMonsterIndex[i], new ObjcetPool<Monster>());

            MonsterData _monsterData = monsterTable.Get_Data(waveData.nArrMonsterIndex[i]);
            dicMonster[waveData.nArrMonsterIndex[i]].Init(_monsterData.obj, _nCount, transform);
        }
        this.arrRouteObj = arrRouteObj;
    }

    public Monster Create_Monster(int nIndex)
    {
        Monster _monster = dicMonster[nIndex].Get();
        if (_monster == null)
            return null;

        _monster.transform.position = arrRouteObj[0].transform.position;
        _monster.Init(nIndex, this);
        _monster.Set_FSM(StateType.Move);

        lisActiveMonster.Add(_monster);
        return _monster;
    }
    public GameObject Get_Route(int nIndex)
    {
        if (arrRouteObj.Length <= nIndex)
            return null;

        return arrRouteObj[nIndex];
    }
    public List<Monster> Get_lisMonster()
    {
        return lisActiveMonster;
    }
    public void Update_Mgr()
    {
        if (lisActiveMonster.Count > 0)
        {
            for (int i = lisActiveMonster.Count - 1; i >= 0; --i)
            {
                if (lisActiveMonster[i].bDie)
                {
                    dicMonster[lisActiveMonster[i].nIndex].Return(lisActiveMonster[i]);
                    lisActiveMonster.RemoveAt(i);
                    continue;
                }

                lisActiveMonster[i].Update_Monster();
            }
        }
    }

    public MonsterData Get_Data(int nIndex)
    {
        return monsterTable.Get_Data(nIndex);
    }
    public void End_Move()
    {
        int _nLife = gameInfo.Get_Life() - 1;
        gameInfo.Set_Life(_nLife);
    }
}
