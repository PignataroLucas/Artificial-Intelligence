using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class SeekState <T> : States <T>
{
    private AI _ai;

    private int randomIndex;

    public SeekState(AI ai) 
    {
        _ai = ai; 
    }

    public override void OnEnter()
    {
        _ai.animator.SetBool("CanRun", true);
        _ai.animator.SetBool("canIdle", false);
    }

    public override void OnUpdate()
    {
        if (_ai.enemiesInRandius.Length > 0) 
        {            
            EventManager.TriggerEvent(GenericEvents.ChangeState, new Hashtable() {
            { GameplayHashtableParameters.ChangeState.ToString(),State.IdleAttack},
            { GameplayHashtableParameters.Agent.ToString(), _ai }
            });
            _ai.animator.SetBool("canIdle", false);
            _ai.animator.SetBool("CanRun", false);
            _ai.animator.SetBool("ToIdleAttack", true);
        }

        _ai.transform.LookAt(_ai.enemyTarget.transform.position);
        _ai.navMeshAgent.destination = _ai.enemyTarget.transform.position;
    }

}
