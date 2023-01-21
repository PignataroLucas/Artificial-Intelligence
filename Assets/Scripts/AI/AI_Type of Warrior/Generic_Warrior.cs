using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Warrior : AI
{

    public override void OnUpdate()
    {
        fsm.OnUpdate();
        Debug.Log(fsm.current);
    }

   
}
