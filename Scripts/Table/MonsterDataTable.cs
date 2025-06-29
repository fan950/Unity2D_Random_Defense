using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MonsterTable", menuName = "GameTables/MonsterTable")]

public class MonsterDataTable : ScriptableObject
{
    public List<MonsterData> lisMonsterData;
    private Dictionary<int, MonsterData> dicMonsterData;

    public void Init()
    {
        dicMonsterData = new Dictionary<int, MonsterData>();

        for (int i = 0; i < lisMonsterData.Count; ++i)
        {
            dicMonsterData.Add(lisMonsterData[i].nIndex, lisMonsterData[i]);
        }
    }

    public MonsterData Get_Data(int nIndex)
    {
        return dicMonsterData[nIndex];
    }
}

[System.Serializable]
public class MonsterData
{
    public int nIndex;
    public string sName;

    public int nHp;
    public float fSpeed;
    public GameObject obj;
}
