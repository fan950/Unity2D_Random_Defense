using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GuardTable", menuName = "GameTables/GuardTable")]

public class GuardDataTable : ScriptableObject
{
    public List<GuardData> lisGuardData;
    private Dictionary<int, GuardData> dicGuardData;

    public void Init()
    {
        dicGuardData = new Dictionary<int, GuardData>();

        for (int i = 0; i < lisGuardData.Count; ++i)
        {
            dicGuardData.Add(lisGuardData[i].nIndex, lisGuardData[i]);
        }
    }

    public GuardData Get_Data(int nIndex)
    {
        return dicGuardData[nIndex];
    }
}

[System.Serializable]
public class GuardData
{
    public int nIndex;
    public float fMoveSpeed;
    public float fAttackRange;
    public float fAttackSpeed;
    public GameObject obj;
}
