using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanDebug : MonoBehaviour , IUpdate
{


    private void Awake()
    {
        UpdateManager.Instance.AddUpdate(this);
    }

    private void CallEventDebug()
    {        
        EventManager.TriggerEvent(GenericEvents.genericDebug);        
    }

    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CallEventDebug();
    }

    private void OnDisable()
    {
        UpdateManager.Instance.RemoveUpdate(this);
    }

}
