using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIEventManager
{
    public void Subscribe(string eventId, Action<string> action);
    public void Unsubscribe(string eventId, Action<string> action);
    public void Subscribe(string eventId, Action action);
    public void Unsubscribe(string eventId, Action action);
    public void Trigger(string eventId,string sValue);
    public void Trigger(string eventId);
}
public class UIEventManager : Singleton<UIEventManager>, IUIEventManager
{
    public Dictionary<string, Action> dicEvent = new Dictionary<string, Action>();
    public Dictionary<string, Action<string>> dicStringEvent = new Dictionary<string, Action<string>>();

    public void Subscribe(string eventId, Action action)
    {
        if (!dicEvent.ContainsKey(eventId))
        {
            dicEvent.Add(eventId, null);
        }

        dicEvent[eventId] += action;
    }

    public void Unsubscribe(string eventId, Action action)
    {
        if (dicEvent.ContainsKey(eventId))
            dicEvent[eventId] -= action;
    }
    public void Trigger(string eventId)
    {
        if (dicEvent.ContainsKey(eventId))
            dicEvent[eventId]();
    }
    public void Subscribe(string eventId, Action<string> action)
    {
        if (!dicStringEvent.ContainsKey(eventId))
        {
            dicStringEvent.Add(eventId, null);
        }

        dicStringEvent[eventId] += action;
    }

    public void Unsubscribe(string eventId, Action<string> action)
    {
        if (dicEvent.ContainsKey(eventId))
            dicStringEvent[eventId] -= action;
    }

    public void Trigger(string eventId,string sValue)
    {
        if (dicStringEvent.ContainsKey(eventId))
            dicStringEvent[eventId](sValue);
    }
}
