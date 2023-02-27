using System.Collections.Generic;
using UnityEngine;

public class Generic_Warrior : AI 
{
    public override void OnUpdate()
    {
        Fsm.OnUpdate();
        
        if (genericSo.Class == TypeOfWarriors.Dwarf)
        {
            enemiesInRandius = Physics.OverlapSphere(transform.position, genericSo.detectionRadius, goblin);
            Debug.Log("Vida de Dwarf : "  + UnitStat.Life);
            Debug.Log("Estado actual de Dwarf :  " + Fsm.Current);
        }
        else if (genericSo.Class == TypeOfWarriors.Goblin)
        {
            enemiesInRandius = Physics.OverlapSphere(transform.position, genericSo.detectionRadius, dwarf);
            Debug.Log("Estado actual de Goblin :  " + Fsm.Current);
        }

        if (enemyTarget != null)
        {
            transform.LookAt(enemyTarget.transform.position);
        }
        
        
        
    }
}
