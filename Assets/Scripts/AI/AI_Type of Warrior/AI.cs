using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI : MonoBehaviour, IUpdate
{

    public FSM<string> fsm;
    private IdleState<string> _idleState;

    public Animator animator;

    public virtual void Awake()
    {
       
       animator = GetComponent<Animator>();
       _idleState = new IdleState<string>(this);
       fsm = new FSM<string>(_idleState);     
    }

    private void Start()
    {
        UpdateManager.Instance.AddUpdate(this);
    }

    public virtual void OnUpdate()
    {
        fsm.OnUpdate();        
    }

    public void LoopAnimations() 
    {
        _idleState.SetTransitionAnim();
    }


    private void OnDisable()
    {
        UpdateManager.Instance.RemoveUpdate(this);
    }

}
