using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventListener
{
    void OnEnableListenerSubscriptions();

    void OnDisableListenerSubscriptions();

}

public static class EventTriggers 
{

    public static void TriggerEvent<T>(GenericEvents eventName,string parameterName,T parameter)
    {
        EventManager.TriggerEvent(eventName, new Hashtable() { { parameterName, parameter } });
    }

    public static void TriggerEvent(GenericEvents eventName) 
    {
        EventManager.TriggerEvent(eventName,null);
    }

}
