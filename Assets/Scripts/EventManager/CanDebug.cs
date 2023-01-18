using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanDebug : MonoBehaviour
{


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            CallEventDebug();
    }

    private void CallEventDebug()
    {        
        EventManager.TriggerEvent(GenericEvents.genericDebug);        
    }
}
