using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WaveTable", menuName = "GameTables/WaveTable")]

public class WaveDataTable : ScriptableObject
{
    public List<WaveData> lisWaveData;

    private Dictionary<int, WaveData> dicWaveData;

    public void Init()
    {
        dicWaveData = new Dictionary<int, WaveData>();

        for (int i = 0; i < lisWaveData.Count; ++i)
        {
            dicWaveData.Add(lisWaveData[i].nIndex, lisWaveData[i]);
        }
    }
    public WaveData Get_Data(int nIndex)
    {
        return dicWaveData[nIndex];
    }
}

[System.Serializable]
public class WaveData
{
    public int nIndex;
    public int[] nArrMonsterIndex;
    public int nLife;
    public int nGold;
    public float fCoolTimeDown;
    public float fEndTime;
}
