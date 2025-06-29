using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SkillTarget 
{
    Target,
    NonTarget,
}
[CreateAssetMenu(fileName = "SkillTable", menuName = "GameTables/SkillTable")]

public class SkillDataTable : ScriptableObject
{
    public List<SkillData> lisSkillData;

    private Dictionary<int, SkillData> dicSkillData;

    public void Init()
    {
        dicSkillData = new Dictionary<int, SkillData>();

        for (int i = 0; i < lisSkillData.Count; ++i)
        {
            dicSkillData.Add(lisSkillData[i].nIndex, lisSkillData[i]);
        }
    }

    public SkillData Get_Data(int nIndex)
    {
        if (dicSkillData == null)
            Init();

        foreach (KeyValuePair<int, SkillData> data in dicSkillData)
        {
            if (data.Value.nIndex == nIndex)
                return data.Value;
        }

        return null;
    }
    public SkillData Get_Data(string sName)
    {
        if (dicSkillData == null)
            Init();

        foreach (KeyValuePair<int, SkillData> data in dicSkillData)
        {
            if (data.Value.sName == sName)
                return data.Value;
        }

        return null;
    }
}

[System.Serializable]
public class SkillData
{
    public int nIndex;
    public string sName;
    public int nDamage;
    public float fDamageSize;
    public int nCoolTimeDown;
    public SkillTarget skillTarget;
    public GameObject obj;
    public int nCount;
}
