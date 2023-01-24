using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState <T> : States<T>
{
    private AI _ai;

    public AttackState (AI ai) 
    {
        _ai = ai; 
    }

    public override void OnEnter()
    {
        SetAnimsTransitions();
    }    

    public void SetAnimsTransitions()
    {
        _ai.animator.SetInteger("attackNum", Random.Range(0, 4));
        _ai.animator.SetBool("canAttack", true);
        _ai.animator.SetBool("ToIdleAttack", false);
    }
    public void ToIdleAttackState()
    {      

        EventManager.TriggerEvent(GenericEvents.ChangeState, new Hashtable() {
        { GameplayHashtableParameters.ChangeState.ToString(),State.IdleAttack},
        { GameplayHashtableParameters.Agent.ToString(), _ai }
        });

       _ai.animator.SetBool("canAttack", false);
       _ai.animator.SetBool("ToIdleAttack", true);

    }



}
