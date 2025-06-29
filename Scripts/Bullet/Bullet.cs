using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct PositionComponent
{
    public Vector3 vecPos;
    public float fAngle;
}
public struct StraightComponent
{
    public Vector3 value;
}
public struct FollowComponent
{
    public Transform target;
}
public struct FixComponent
{
    public Vector3 value;
}
public struct SpeedComponent
{
    public float fSpeed;
}
public struct ColliderComponent
{
    public float size;
}
public struct DamageComponent
{
    public DamageType damageType;
    public int value;
    public float fSize;
}
public struct FxComponent
{
    public string parh;
}
public struct ObjComponent
{
    public int nIndex;
    public GameObject bullet;
}
public struct LifeTimeComponent
{
    public float fLifeTime;
}
public struct DestroyComponent { }
public interface IComponentContainer { }
public class ComponentContainer<T> : IComponentContainer where T : struct
{
    public T Value;
}

public class Bullet
{
    public int nIndex;
    private Dictionary<Type, IComponentContainer> lisComponent = new Dictionary<Type, IComponentContainer>();
    public void Add<T>(T component) where T : struct
    {
        var type = typeof(T);
        if (!lisComponent.TryGetValue(typeof(T), out var container))
        {
            container = new ComponentContainer<T>();
            lisComponent[type] = container;
        }
          ((ComponentContainer<T>)lisComponent[type]).Value = component;
    }
    public T Get<T>() where T : struct
    {
        return ((ComponentContainer<T>)lisComponent[typeof(T)]).Value;
    }
    public bool Has<T>() where T : struct
    {
        return lisComponent.ContainsKey(typeof(T));
    }

    public void Set<T>(T component) where T : struct
    {
        if (lisComponent.TryGetValue(typeof(T), out var container))
        {
            ((ComponentContainer<T>)container).Value = component;
        }
    }

    public void Remove<T>() where T : struct
    {
        lisComponent.Remove(typeof(T));
    }
}
