using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletTable", menuName = "GameTables/BulletTable")]
public class BulletDataTable : ScriptableObject
{
    public List<BulletData> lisBulletData;

    private Dictionary<int, BulletData> dicBulletData;

    public void Init()
    {
        dicBulletData = new Dictionary<int, BulletData>();

        for (int i = 0; i < lisBulletData.Count; ++i)
        {
            dicBulletData.Add(lisBulletData[i].nIndex, lisBulletData[i]);
        }
    }

    public BulletData Get_Bullet(int nIndex)
    {
        if (dicBulletData == null)
            Init();

        foreach (KeyValuePair<int, BulletData> data in dicBulletData)
        {
            if (data.Value.nIndex == nIndex)
                return data.Value;
        }

        return null;
    }
}

[System.Serializable]
public class BulletData
{
    public int nIndex;
    public float fSpeed;
    public GameObject obj;
    public string sFxPath;
    public BulletType bulletType;
}
