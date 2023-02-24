using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public struct UnitStat
{
    public int AttackDamage;
    public int Life;
    public float Speed;
}
public abstract class AI : MonoBehaviour, IUpdate, IEventListener, IGridEntity
{

    public FSM<string> Fsm;

    private IdleState<string> _idleState;
    private IdleAttackState<string> _idleAttackState;
    private AttackState<string> _attackState;
    private SeekState<string> _seekState;
    protected DeadState<string> _deadState;

    public Generic_Unit_SO genericSo;
    public UnitStat UnitStat;

    public Animator animator;

    public LayerMask goblin,dwarf;

    public List <GameObject> target;
    public GameObject enemyTarget;
    public Collider [] enemiesInRandius; 

    public NavMeshAgent navMeshAgent;
    
    public ConeQuery coneQuery;


    public int _currentTargetIndex = -1;
    public bool canToIdleAttack;
    
    [SerializeField] public List<GameObject> dwarfUnits = new List<GameObject>();
    [SerializeField] public List<GameObject> goblinUnits = new List<GameObject>();

    public event System.Action<IGridEntity> OnMove;

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
        _deadState = new DeadState<string>(this);
        
        
        _idleState.SetTransition(State.Seek, _seekState);
        _seekState.SetTransition(State.IdleAttack, _idleAttackState);
        _idleAttackState.SetTransition(State.Dead,_deadState);
        _idleAttackState.SetTransition(State.Attack, _attackState);
        _attackState.SetTransition(State.Dead,_deadState);
        _attackState.SetTransition(State.IdleAttack, _idleAttackState);
        _attackState.SetTransition(State.Idle,_idleState);
        _attackState.SetTransition(State.Seek,_seekState);
        
        

        Fsm = new FSM<string>(_idleState);

        navMeshAgent = GetComponent<NavMeshAgent>();

        UnitStat = new UnitStat
        {
            AttackDamage = genericSo.AttackDamage,
            Life = genericSo.Life,
            Speed = genericSo.Speed,
        };

        canToIdleAttack = true;

    }
    public virtual void OnUpdate() { }
    private void TransitionState(Hashtable data)
    {
        string state = data[GameplayHashtableParameters.ChangeState.ToString()].ToString();
                
        AI ai = (AI)data[GameplayHashtableParameters.Agent.ToString()];

        if (ai == this) {

            Fsm.Transition(state);           
        } 
    }
    public  virtual void SetTarget()
    {

    }

    #region AnimationEvents

    public void LoopAnimations() 
    {       
        _idleState.SetTransitionAnim();  
    }
    public void Attack()
    {
        animator.SetBool("canIdle",true);
        _idleAttackState.ToAttackState();
    }
    public void ToIdleAttack()
    {
        _attackState.ToIdleAttackState();
    }
    public void Detect()
    {
        _attackState.Detect();
    }

    #endregion
    private void OnDisable()
    {
        UpdateManager.Instance.RemoveUpdate(this);
        OnDisableListenerSubscriptions();
    }
    public void OnEnableListenerSubscriptions()
    {
        EventManager.StartListening(GenericEvents.ChangeState, TransitionState);
        EventManager.StartListening(GenericEvents.RandomTargets, SetRandomTarget);
        EventManager.StartListening(GenericEvents.ChangeToSeekState, ChangeToSeekState);
       
    }
    public void OnDisableListenerSubscriptions()
    {
        EventManager.StopListering(GenericEvents.ChangeState, TransitionState);
        EventManager.StopListering(GenericEvents.RandomTargets, SetRandomTarget);
        EventManager.StopListering(GenericEvents.ChangeToSeekState, ChangeToSeekState);
        
    }
    private void SetRandomTarget(Hashtable obj)
    {
        if (genericSo.Class == TypeOfWarriors.Dwarf)
        {            
            if (enemyTarget == null)
            {
                int randomIndex = Random.Range(0, goblinUnits.Count);
                enemyTarget = goblinUnits[randomIndex];
                _currentTargetIndex = 0;
            }
        }
        else if (genericSo.Class == TypeOfWarriors.Goblin)
        {
            if (enemyTarget == null)
            {
                int randomIndex = Random.Range(0, target.Count);
                enemyTarget = target[randomIndex];
                _currentTargetIndex = 0;
            }
        }
    }
    private void ChangeToSeekState(Hashtable obj)
    {
        EventManager.TriggerEvent(GenericEvents.ChangeState, new Hashtable() {
            { GameplayHashtableParameters.ChangeState.ToString(),State.Seek},
            { GameplayHashtableParameters.Agent.ToString(), this }
            });
    }
    
    /*public void AddTarget(GameObject _target)
    {
        //this.target.Add(_target);   
    }*/
    
    public void SetDwarfUnits(List<GameObject> newUnits) 
    {
        dwarfUnits = newUnits;
    }
    public void SetGoblinUnits(List<GameObject> newUnits) 
    {
        goblinUnits = newUnits;
    }
    
    public Vector3 Position { get => transform.position; set => transform.position = value; }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, genericSo.detectionRadius);
    }
}
