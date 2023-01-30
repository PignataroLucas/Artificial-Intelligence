using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SeekState <T> : States <T>
{
    private AI _ai;
   

    public SeekState(AI ai) 
    {
        _ai = ai; 
    }

    public override void OnEnter()
    {
        int randomIndex = Random.Range(0, _ai.target.Length);

        _ai._navMeshAgent.destination = _ai.target[randomIndex].transform.position;
        _ai.transform.LookAt(_ai.target[randomIndex].transform.position);
        _ai.animator.SetBool("CanRun", true);
        _ai.animator.SetBool("canIdle", false);
    }

    public override void OnUpdate()
    {
       Collider[] enemiesInRandius = Physics.OverlapSphere(_ai.transform.position, _ai._genericSO.detectionRadius, _ai.goblin);

        if (enemiesInRandius.Length > 0) 
        {            
            EventManager.TriggerEvent(GenericEvents.ChangeState, new Hashtable() {
            { GameplayHashtableParameters.ChangeState.ToString(),State.IdleAttack},
            { GameplayHashtableParameters.Agent.ToString(), _ai }
            });
            _ai.animator.SetBool("canIdle", false);
        }
    }

}
