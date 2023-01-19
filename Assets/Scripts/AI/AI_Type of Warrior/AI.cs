using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI : MonoBehaviour, IUpdate , IEventListener
{

    public FSM<string> fsm;
    private IdleState<string> _idleState;
    private IdleAttackState<string> _idleAttackState;

    public Animator animator;

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

        _idleState.SetTransition(State.IdleAttack, _idleAttackState);

        fsm = new FSM<string>(_idleState);
    }

    public virtual void OnUpdate()
    {
        fsm.OnUpdate();

        Debug.Log(fsm.current);
    }

    private void TransitionState(Hashtable data)
    {
        string state = data[GameplayHashtableParameters.ChangeState.ToString()].ToString();
                
        AI ai = (AI)data[GameplayHashtableParameters.Agent.ToString()];

        if (ai == this) {

            fsm.Transition(state);
            Debug.Log("Entre");
        } 


    }


    public void LoopAnimations() 
    {
        _idleState.SetTransitionAnim();
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



}
