using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class AI : MonoBehaviour, IUpdate , IEventListener
{

    public FSM<string> fsm;
    private IdleState<string> _idleState;
    private IdleAttackState<string> _idleAttackState;
    private AttackState<string> _attackState;

    public Generic_Unit_SO _genericSO;

    public Animator animator;

    public LayerMask goblin;
   

    public virtual void Awake()
    {       
       animator = GetComponent<Animator>();
       OnEnableListenerSubscriptions();
    }

    private void Start()
    {
        UpdateManager.Instance.AddUpdate(this);

        _idleState = new IdleState<string>(this);
        _idleAttackState = new IdleAttackState<string>(this);
        _attackState = new AttackState<string>(this);

        _idleState.SetTransition(State.IdleAttack, _idleAttackState);
        _idleAttackState.SetTransition(State.Attack, _attackState);
        _attackState.SetTransition(State.IdleAttack, _idleAttackState);

        fsm = new FSM<string>(_idleState);
    }

    public virtual void OnUpdate()
    {
       
    }

    private void TransitionState(Hashtable data)
    {
        string state = data[GameplayHashtableParameters.ChangeState.ToString()].ToString();
                
        AI ai = (AI)data[GameplayHashtableParameters.Agent.ToString()];

        if (ai == this) {

            fsm.Transition(state);           
        } 
    }


    public void LoopAnimations() 
    {       
        _idleState.SetTransitionAnim();  
    }

    public void Attack()
    {
        _idleAttackState.ToAttackState();
    }

    public void ToIdleAttack()
    {
        _attackState.ToIdleAttackState();
    }

    private void OnDisable()
    {
        UpdateManager.Instance.RemoveUpdate(this);
        OnDisableListenerSubscriptions();
    }

   

    public void OnEnableListenerSubscriptions()
    {
        EventManager.StartListening(GenericEvents.ChangeState, TransitionState);       
    }

    

    public void OnDisableListenerSubscriptions()
    {
        EventManager.StartListening(GenericEvents.ChangeState, TransitionState);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _genericSO.detectionRadius);

    }

}
