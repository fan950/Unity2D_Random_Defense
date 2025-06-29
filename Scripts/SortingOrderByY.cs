using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrderByY : MonoBehaviour
{
    public int nSortingOffset = 0;
    private SpriteRenderer[] arrSprite;
    private float fMultiplier = 100f;

    void Awake()
    {
        arrSprite = gameObject.GetComponentsInChildren<SpriteRenderer>();
    }

    void LateUpdate()
    {
        for (int i = 0; i < arrSprite.Length; ++i)
        {
            arrSprite[i].sortingOrder = Mathf.RoundToInt(-transform.position.y * fMultiplier) + nSortingOffset;
        }
    }
}
