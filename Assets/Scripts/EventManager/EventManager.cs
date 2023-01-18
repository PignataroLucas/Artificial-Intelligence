using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    private Dictionary<GenericEvents, Action<Hashtable>> eventsGenericDictionary;

    private static EventManager _eventManager;

    public static EventManager Instance 
    {
        get 
        {
            if (!_eventManager) 
            {
                _eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                //if (_eventManager) DontDestroyOnLoad(_eventManager.gameObject);

                if (!_eventManager){Debug.LogError("You need the object EventManager with the script in the scene"); }
                else
                {
                    _eventManager.Init();
                }
            }
            return _eventManager;
        }
    }

    void Init() 
    {
        if(eventsGenericDictionary == null) { eventsGenericDictionary=new Dictionary<GenericEvents, Action<Hashtable>>();}
    }

    public static void StartListening(GenericEvents eventName,Action<Hashtable> listener) 
    {
        if (Instance.eventsGenericDictionary.ContainsKey(eventName)) Instance.eventsGenericDictionary[eventName] += listener;
        else Instance.eventsGenericDictionary.Add(eventName, listener);
    }

    public static void StopListering(GenericEvents eventName,Action<Hashtable> listener) 
    {
        if (Instance.eventsGenericDictionary.ContainsKey(eventName)) Instance.eventsGenericDictionary[eventName] -= listener;
    }

    public static void TriggerEvent(GenericEvents eventName,Hashtable eventParams = default(Hashtable)) 
    {
        Action<Hashtable> thisEvent = null;
        if (Instance.eventsGenericDictionary.TryGetValue(eventName,out thisEvent))
        {
            if (thisEvent != null) thisEvent.Invoke(eventParams);
        }
    }


}
