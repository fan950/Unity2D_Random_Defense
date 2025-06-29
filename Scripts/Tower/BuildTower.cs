using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTower : MonoBehaviour
{
    public SpriteRenderer cloneTower;

    public void On_Clone(Sprite sprite)
    {
        cloneTower.sprite = sprite;
        cloneTower.gameObject.SetActive(true);
    }

    public void Off_Clone()
    {
        cloneTower.gameObject.SetActive(false);
    }
}
