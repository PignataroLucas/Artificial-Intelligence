using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;


public abstract class AI : MonoBehaviour, IUpdate , IEventListener
{

    public FSM<string> fsm;

    private IdleState<string> _idleState;
    private IdleAttackState<string> _idleAttackState;
    private AttackState<string> _attackState;
    private SeekState<string> _seekState;

    public Generic_Unit_SO _genericSO;

    public Animator animator;

    public LayerMask goblin,dwarf;

    public GameObject [] target;
    public GameObject enemyTarget;
    public Collider [] enemiesInRandius; 

    public NavMeshAgent _navMeshAgent;
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
        _seekState = new SeekState<string>(this);

        
        _idleAttackState.SetTransition(State.Attack, _attackState);
        _attackState.SetTransition(State.IdleAttack, _idleAttackState);
        _idleState.SetTransition(State.Seek, _seekState);
        _seekState.SetTransition(State.IdleAttack, _idleAttackState);

        fsm = new FSM<string>(_idleState);

        _navMeshAgent = GetComponent<NavMeshAgent>();       
    }

    public virtual void OnUpdate()
    { }

    private void TransitionState(Hashtable data)
    {
        string state = data[GameplayHashtableParameters.ChangeState.ToString()].ToString();
                
        AI ai = (AI)data[GameplayHashtableParameters.Agent.ToString()];

        if (ai == this) {

            fsm.Transition(state);           
        } 
    }

    public  virtual void SetTarget()
    {

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
