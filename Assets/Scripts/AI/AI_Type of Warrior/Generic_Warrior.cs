using System.Collections.Generic;
using UnityEngine;

public class Generic_Warrior : AI 
{
    public override void OnUpdate()
    {
        Fsm.OnUpdate();
        
        if (genericSo.Class == TypeOfWarriors.Dwarf)
        {
            Debug.Log(Fsm.Current);
            enemiesInRandius = Physics.OverlapSphere(transform.position, genericSo.detectionRadius, goblin);
            //target = new List<GameObject>(GameObject.FindGameObjectsWithTag("Goblin"));
        }
        else if (genericSo.Class == TypeOfWarriors.Goblin)
        {
            enemiesInRandius = Physics.OverlapSphere(transform.position, genericSo.detectionRadius, dwarf);
            target = new List<GameObject>(GameObject.FindGameObjectsWithTag("Dwarf"));
        }

        if (enemyTarget != null)
        {
            transform.LookAt(enemyTarget.transform.position);
            //return;
        }
        
    }
}
