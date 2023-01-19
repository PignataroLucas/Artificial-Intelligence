using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour, IEventListener
{

    private void Awake()
    {
        OnEnableListenerSubscriptions();
    }

    private void DebugCall(Hashtable hashtable)
    {
        Debug.Log("Debug work OK");
    }


    public void OnEnableListenerSubscriptions()
    {
        EventManager.StartListening(GenericEvents.GenericDebug, DebugCall);
    }


    public void OnDisableListenerSubscriptions()
    {
        EventManager.StopListering(GenericEvents.GenericDebug, DebugCall);
    }

    private void OnDisable()
    {
        OnDisableListenerSubscriptions();
    }

}
