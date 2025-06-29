using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlace : MonoBehaviour
{
    [SerializeField] private BuildTower buildTower;
    [HideInInspector] public Tower tower;
    public int nIndex { get { return tower.nIndex; } }
    public int nLevel
    {
        get
        {
            if (tower == null)
                return 1;
            return tower.nLevel;
        }
    }
    public Vector3 Get_Pos()
    {
        return transform.position;
    }
    public void Create_Tower(Tower tower)
    {
        buildTower.gameObject.SetActive(false);

        this.tower = tower;
    }
    public void Destroy_Tower()
    {
        buildTower.gameObject.SetActive(true);
        tower = null;
    }
    public bool Is_TowerObj(GameObject obj)
    {
        if (tower == null)
            return false;

        return tower.gameObject == obj;
    }
}
