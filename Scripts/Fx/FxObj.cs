using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxObj : MonoBehaviour
{
    public string sId;
    public float fMaxLifeTime;
    protected float fLifeTime;

    public virtual void OnEnable()
    {
        fLifeTime = 0;
    }

    public virtual void Update()
    {
        fLifeTime += Time.deltaTime;
        if (fLifeTime >= fMaxLifeTime) 
        {
            gameObject.SetActive(false);
        }
    }
}
