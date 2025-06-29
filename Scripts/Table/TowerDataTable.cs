using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CreateType 
{
    Bullet,
    Guard,
}
public enum DamageType
{
    Single,
    Multi,
}
[CreateAssetMenu(fileName = "TowerTable", menuName = "GameTables/TowerTable")]
public class TowerDataTable : ScriptableObject
{
    public List<TowerData> lisTowerData;

    private Dictionary<int, TowerData> dicTowerData;

    public void Init()
    {
        dicTowerData = new Dictionary<int, TowerData>();

        for (int i = 0; i < lisTowerData.Count; ++i)
        {
            dicTowerData.Add(lisTowerData[i].nIndex, lisTowerData[i]);
        }
    }

    public TowerData Get_Data(int nIndex)
    {
        return dicTowerData[nIndex];
    }
}


[System.Serializable]
public class TowerData
{
    public int nIndex;
    public string sName;
    public int nLevel;
    public int nAttack;
    public float fAttackSpeed;
    public float fAttackRange;
    public GameObject towerObj;
    public CreateType createType;
    public DamageType damageType;
    public int nBulletIndex;
}
