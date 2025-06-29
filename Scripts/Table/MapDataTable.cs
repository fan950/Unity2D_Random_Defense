using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapTable", menuName = "GameTables/MapTable")]
public class MapDataTable : ScriptableObject
{
    public List<MapData> lisMapData;

    private Dictionary<int, MapData> dicMapData;

    public void Init()
    {
        dicMapData = new Dictionary<int, MapData>();

        for (int i = 0; i < lisMapData.Count; ++i)
        {
            dicMapData.Add(lisMapData[i].nIndex, lisMapData[i]);
        }
    }

    public MapData Get_Map(int nIndex)
    {
        if (dicMapData == null)
            Init();

        foreach (KeyValuePair<int, MapData> data in dicMapData)
        {
            if (data.Value.nIndex == nIndex)
                return data.Value;
        }

        return null;
    }
    public MapData Get_Map(string sName)
    {
        if (dicMapData == null)
            Init();

        foreach (KeyValuePair<int, MapData> data in dicMapData)
        {
            if (data.Value.sName == sName)
                return data.Value;
        }

        return null;
    }
}

[System.Serializable]
public class MapData
{
    public int nIndex;
    public string sName;
    public string sScene;
    public Sprite mapSprite;
}
