using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFx : FxObj
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float fScaleTime;
    private const float fSpeed = 3;

    public override void Update()
    {
        base.Update();

        fScaleTime += Time.deltaTime * fSpeed;
        if (fScaleTime < 1)
        {
            transform.localScale = new Vector3(fScaleTime, fScaleTime, fScaleTime);
            spriteRenderer.color = new Color(1, 1, 1, fScaleTime);
        }
        else
        {
            fLifeTime += Time.deltaTime;
            if (fLifeTime >= fMaxLifeTime)
            {
                gameObject.SetActive(false);
            }
        }
    }
    public override void OnEnable()
    {
        fLifeTime = 0;
        fScaleTime = 0;
        transform.localScale = Vector3.zero;
        spriteRenderer.color = new Color(1, 1, 1, 0);
    }
}
