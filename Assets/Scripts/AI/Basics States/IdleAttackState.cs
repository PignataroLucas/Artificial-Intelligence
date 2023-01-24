using System;
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
        SetAnimations();
        Debug.Log("Idle Attack");
    }


    private void SetAnimations()
    {        
        _ai.animator.SetBool("ToIdleAttack", true);
        _ai.animator.SetBool("canIdle", false);
    }
    public  void ToAttackState()
    {
        EventManager.TriggerEvent(GenericEvents.ChangeState, new Hashtable() {
        { GameplayHashtableParameters.ChangeState.ToString(),State.Attack},
        { GameplayHashtableParameters.Agent.ToString(), _ai }
        });
        _ai.animator.SetBool("ToIdleAttack", false);
        _ai.animator.SetBool("canIdle", false);
    }

    
}
