using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAttackState <T> : States<T>
{

    private AI _ai;

    public IdleAttackState(AI ai)
    {
        _ai = ai;
    }

    public override void OnEnter()
    {
        Debug.Log("IdleAttack");
    }




}
