using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITowerManager
{
    public void Create_BaseTower();
    public void Destroy_Tower(GameObject obj);
    public void Upgrade_Tower(GameObject obj);
    public List<Tower> Get_lisTower();
}
public class TowerManager : Singleton<TowerManager>, ITowerManager
{
    private Dictionary<int, ObjcetPool<Tower>> dicTower = new Dictionary<int, ObjcetPool<Tower>>();

    private List<Tower> lisActiveTower = new List<Tower>();
    private List<Tower> lisDestroyTower = new List<Tower>();

    private BuildingPlace[] arrBuildingPlace;
    private TowerDataTable towerDataTable;

    private const string sTablePath = "Table/TowerTable";

    public void Set_Tower()
    {
        GameObject[] _obj = GameObject.FindGameObjectsWithTag("Building");
        arrBuildingPlace = new BuildingPlace[_obj.Length];
        for (int i = 0; i < _obj.Length; ++i)
        {
            arrBuildingPlace[i] = _obj[i].GetComponent<BuildingPlace>();
        }

        towerDataTable = Resources.Load(sTablePath) as TowerDataTable;
        towerDataTable.Init();

        int _nCount = 5;
        for (int i = 0; i < towerDataTable.lisTowerData.Count; ++i)
        {
            if (!dicTower.ContainsKey(towerDataTable.lisTowerData[i].nIndex))
                dicTower.Add(towerDataTable.lisTowerData[i].nIndex, new ObjcetPool<Tower>());
            dicTower[towerDataTable.lisTowerData[i].nIndex].Init(towerDataTable.lisTowerData[i].towerObj, _nCount, transform);
        }
    }
    public TowerData Get_Data(int nIndex)
    {
        return towerDataTable.Get_Data(nIndex);
    }

    public BuildingPlace Find_RandomBuild()
    {
        List<BuildingPlace> _lisPlace = new List<BuildingPlace>();
        for (int i = 0; i < arrBuildingPlace.Length; ++i)
        {
            if (arrBuildingPlace[i].tower == null)
                _lisPlace.Add(arrBuildingPlace[i]);
        }

        if (_lisPlace.Count == 0)
            return null;
        else
            return _lisPlace[UnityEngine.Random.Range(0, _lisPlace.Count)];
    }
    public void Create_BaseTower()
    {
        BuildingPlace _build = Find_RandomBuild();

        if (_build == null)
            return;

        int _nIndex = UnityEngine.Random.Range(1, 6);
        Create_Tower(_nIndex, _build);
    }
    private void Create_Tower(int nIndex, BuildingPlace buildingPlace)
    {
        Tower _tower = dicTower[nIndex].Get();
        TowerData _towerData = Get_Data(nIndex);
        switch (_towerData.createType)
        {
            case CreateType.Bullet:
                _tower.Init(_towerData, BulletManager.Instance);
                break;
            case CreateType.Guard:
                _tower.Init(_towerData, GuardManager.Instance);
                break;
        }
        _tower.transform.position = buildingPlace.Get_Pos();
        buildingPlace.Create_Tower(_tower);

        lisActiveTower.Add(_tower);
    }
    public void Upgrade_Tower(GameObject obj)
    {
        int _nTowerCount = 5;
        for (int i = 0; i < arrBuildingPlace.Length; ++i)
        {
            if (arrBuildingPlace[i].Is_TowerObj(obj))
            {
                arrBuildingPlace[i].tower.Die();
                dicTower[arrBuildingPlace[i].nIndex].Return(arrBuildingPlace[i].tower);

                int _nIndex = UnityEngine.Random.Range(1, _nTowerCount + 1) + ((arrBuildingPlace[i].nLevel) * _nTowerCount);
                Create_Tower(_nIndex, arrBuildingPlace[i]);

                return;
            }
        }
    }
    public int Get_Level(GameObject obj)
    {
        for (int i = 0; i < arrBuildingPlace.Length; ++i)
        {
            if (arrBuildingPlace[i].Is_TowerObj(obj))
            {
                return arrBuildingPlace[i].nLevel;
            }
        }
        return 0;
    }

    public void Destroy_Tower(GameObject obj)
    {
        for (int i = 0; i < arrBuildingPlace.Length; ++i)
        {
            if (arrBuildingPlace[i].Is_TowerObj(obj))
            {
                lisDestroyTower.Add(arrBuildingPlace[i].tower);

                arrBuildingPlace[i].tower.Die();
                dicTower[arrBuildingPlace[i].nIndex].Return(arrBuildingPlace[i].tower);
                arrBuildingPlace[i].Destroy_Tower();
                return;
            }
        }
    }
    public List<Tower> Get_lisTower()
    {
        return lisActiveTower;
    }
    public void Update_Mgr()
    {
        for (int i = 0; i < lisActiveTower.Count; ++i)
        {
            if (lisDestroyTower.Contains(lisActiveTower[i]))
                continue;

            lisActiveTower[i].Update_Tower();
        }

        if (lisDestroyTower.Count > 0)
        {
            for (int i = 0; i < lisDestroyTower.Count; ++i)
            {
                lisActiveTower.Remove(lisDestroyTower[i]);
            }
            lisDestroyTower.Clear();
        }
    }
}
