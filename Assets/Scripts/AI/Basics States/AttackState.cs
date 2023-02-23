using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public override void OnUpdate()
    {
        CheckStatus();
    }

    private void CheckStatus()
    {
        if (_ai.UnitStat.Life <= 0)
        {
            EventManager.TriggerEvent(GenericEvents.ChangeState, new Hashtable() {
                { GameplayHashtableParameters.ChangeState.ToString(),State.Dead},
                { GameplayHashtableParameters.Agent.ToString(), _ai }
            });
        }
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

        if (_ai.canToIdleAttack)
        {
            _ai.animator.SetBool("canAttack", false);
            _ai.animator.SetBool("ToIdleAttack", true);
        }
        else 
        {
            _ai.animator.SetBool("canAttack", false);
            _ai.animator.SetBool("ToIdleAttack", false);
            _ai.animator.SetBool("canIdle", true);
        }
      
    }
    
    public IEnumerable<Generic_Warrior> Detect()
    {
        IEnumerable<Generic_Warrior> detectedWarriors = Enumerable.Empty<Generic_Warrior>();
        if (_ai.genericSo.Class == TypeOfWarriors.Dwarf)
        {
            detectedWarriors = _ai.coneQuery.Query()
                .OfType<Generic_Warrior>()
                .Where(w => w.genericSo.Class == TypeOfWarriors.Goblin );

            foreach (var warrior in detectedWarriors)
            {
                int damage = Random.Range(300,400);
                warrior.UnitStat.Life -= damage;
                if (warrior.UnitStat.Life <= 0)
                {
                    _ai.animator.SetBool("canAttack", false);
                    _ai.animator.SetBool("ToIdleAttack", false);
                    _ai.canToIdleAttack = false;
                    EventManager.TriggerEvent(GenericEvents.ChangeState, new Hashtable() {
                        { GameplayHashtableParameters.ChangeState.ToString(),State.Idle},
                        { GameplayHashtableParameters.Agent.ToString(), _ai }
                    });
                }
            }
        }
        else if (_ai.genericSo.Class == TypeOfWarriors.Goblin)
        {
            detectedWarriors = _ai.coneQuery.Query()
                .OfType<Generic_Warrior>()
                .Where(w => w.genericSo.Class == TypeOfWarriors.Dwarf );

            foreach (var warrior in detectedWarriors)
            { 
                //int damage = Random.Range(300, 400);
                //warrior.UnitStat.Life -= damage;
                //_ai.target.Remove(_ai.enemyTarget);
                //NewTarget();
            }
        }
        return detectedWarriors;
    }
}
