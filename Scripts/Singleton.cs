using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected bool bDestroy;
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject findObj = GameObject.Find(typeof(T).Name);

                if (findObj)
                {
                    instance = findObj.GetComponent<T>();
                }
                else
                {
                    GameObject obj = Resources.Load("Manager/" + typeof(T).Name) as GameObject;
                    if (obj == null)
                    {
                        obj = new GameObject(typeof(T).Name);
                        instance = obj.AddComponent<T>();
                    }
                    else
                    {
                        obj = Instantiate(Resources.Load("Manager/" + typeof(T).Name) as GameObject);
                        obj.name = typeof(T).Name;
                        instance = obj.GetComponent<T>();
                    }
                }
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        if (null == instance)
            instance = GetComponent<T>();

        if (!bDestroy)
            DontDestroyOnLoad(gameObject);
    }
}