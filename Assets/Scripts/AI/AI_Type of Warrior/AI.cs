using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
public abstract class AI : MonoBehaviour, IUpdate, IEventListener, IGridEntity
{

    public FSM<string> Fsm;

    private IdleState<string> _idleState;
    private IdleAttackState<string> _idleAttackState;
    private AttackState<string> _attackState;
    private SeekState<string> _seekState;
    
    public Generic_Unit_SO genericSo;

    public Animator animator;

    public LayerMask goblin,dwarf;

    public GameObject [] target;
    public GameObject enemyTarget;
    public Collider [] enemiesInRandius; 

    public NavMeshAgent navMeshAgent;
    
    public ConeQuery coneQuery;

    public int life;

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
        
        _idleAttackState.SetTransition(State.Attack, _attackState);
        _attackState.SetTransition(State.IdleAttack, _idleAttackState);
        _idleState.SetTransition(State.Seek, _seekState);
        _seekState.SetTransition(State.IdleAttack, _idleAttackState);

        Fsm = new FSM<string>(_idleState);

        navMeshAgent = GetComponent<NavMeshAgent>(); 
        
        life = Random.Range(20,40);
        
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
                int randomIndex = Random.Range(0, target.Length);
                enemyTarget = target[randomIndex];
            }
        }
        else if (genericSo.Class == TypeOfWarriors.Goblin)
        {
            if (enemyTarget == null)
            {
                int randomIndex = Random.Range(0, target.Length);
                enemyTarget = target[randomIndex];
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
    public IEnumerable<Generic_Warrior> Detect()
    {
        IEnumerable<Generic_Warrior> detectedWarriors = Enumerable.Empty<Generic_Warrior>();
        if (genericSo.Class == TypeOfWarriors.Dwarf)
        {
             detectedWarriors = coneQuery.Query()
                 .OfType<Generic_Warrior>()
                 .Where(w => w.genericSo.Class == TypeOfWarriors.Goblin );

             foreach (var warrior in detectedWarriors)
             {
                 int damage = Random.Range(1, 15);
                 warrior.life -= damage;
             }
        }
        else if (genericSo.Class == TypeOfWarriors.Goblin)
        {
            detectedWarriors = coneQuery.Query()
                .OfType<Generic_Warrior>()
                .Where(w => w.genericSo.Class == TypeOfWarriors.Dwarf );
        }
        return detectedWarriors;
    }
    public Vector3 Position { get => transform.position; set => transform.position = value; }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, genericSo.detectionRadius);

    }

    
    
}
