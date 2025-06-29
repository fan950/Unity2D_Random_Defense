using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjcetPool<T> where T : MonoBehaviour
{
    private GameObject obj;
    private Transform parent;
    private const int nTempCount = 5;

    private Stack<T> pool = new Stack<T>();
    private List<T> lisActive = new List<T>();

    public void Init(string sPath, int nCount, Transform parent)
    {
        this.parent = parent;
        obj = Resources.Load(sPath) as GameObject;

        for (int i = 0; i < nCount; ++i)
        {
            Add();
        }
    }
    public void Init(GameObject obj, int nCount, Transform parent)
    {
        this.parent = parent;
        this.obj = obj;

        for (int i = 0; i < nCount; ++i)
        {
            Add();
        }
    }
    public void Add()
    {
        var Instan = Object.Instantiate(obj, parent);
        T poolObj = Instan.GetComponent<T>();
        Instan.transform.localPosition = Vector3.zero;
        Instan.transform.localScale = Vector3.one;
        Instan.name = obj.name;

        Instan.SetActive(false);
        pool.Push(poolObj);
    }

    public T Get(bool bActive = true)
    {
        if (pool.Count <= 0)
        {
            for (int i = 0; i < nTempCount; ++i)
            {
                Add();
            }
        }

        var obj = pool.Pop();
        obj.transform.SetAsLastSibling();
        obj.gameObject.SetActive(bActive);
        lisActive.Add(obj);
        return obj;
    }
    public List<T> Get_ListActive()
    {
        return lisActive;
    }
    public Stack<T> All()
    {
        return pool;
    }
    public void Return(T obj)
    {
        if (obj == null)
            return;

        for (int i = 0; i < lisActive.Count; ++i)
        {
            if (lisActive[i].gameObject == obj.gameObject)
            {
                obj.gameObject.SetActive(false);
                pool.Push(obj);
                lisActive.RemoveAt(i);
                break;
            }
        }
    }
    public void Return_All()
    {
        for (int i = 0; i < lisActive.Count; ++i)
        {
            pool.Push(lisActive[i]);
            lisActive[i].gameObject.SetActive(false);
        }
        lisActive.Clear();
    }

    public void Clear()
    {
        Return_All();
        for (int i = 0; i < pool.Count; ++i)
        {
            Object.Destroy(pool.Pop().gameObject);
        }
        pool.Clear();
    }
}