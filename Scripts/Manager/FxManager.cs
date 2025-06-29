using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxManager : Singleton<FxManager>
{
    private const int nCount = 5;
    private Dictionary<string, ObjcetPool<FxObj>> dicFxObj = new Dictionary<string, ObjcetPool<FxObj>>();

    private const string sFx = "Fx";
    public override void Awake()
    {
        base.Awake();

        var _fxObj = Resources.LoadAll(sFx);

        for (int i = 0; i < _fxObj.Length; ++i)
        {
            FxObj _fx = (_fxObj[i] as GameObject).GetComponent<FxObj>();
            dicFxObj.Add(_fx.sId, new ObjcetPool<FxObj>());
            dicFxObj[_fx.sId].Init(_fx.gameObject, nCount, transform);
        }
    }

    public FxObj Get_Fx(string sId,Vector3 vecPos) 
    {
        FxObj _fx = dicFxObj[sId].Get();
        _fx.transform.position = vecPos; 
        return _fx;
    }
}
