using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Warrior : AI
{
    


    public override void OnUpdate()
    {
        fsm.OnUpdate();
        Debug.Log(fsm.current);
        

        if(_genericSO.Class == TypeOfWarriors.Dwarf)
        {
            enemiesInRandius = Physics.OverlapSphere(transform.position, _genericSO.detectionRadius, goblin);
            target = GameObject.FindGameObjectsWithTag("Goblin");
            if(enemyTarget == null && Input.GetKeyDown(KeyCode.Space)) 
            {
                int randomIndex = Random.Range(0, target.Length);
                enemyTarget = target[randomIndex];
            }
        }
        else if (_genericSO.Class == TypeOfWarriors.Goblin)
        {
            enemiesInRandius = Physics.OverlapSphere(transform.position, _genericSO.detectionRadius, dwarf);
            target = GameObject.FindGameObjectsWithTag("Dwarf");
            if (enemyTarget == null && Input.GetKeyDown(KeyCode.Space))
            {
                int randomIndex = Random.Range(0, target.Length);
                enemyTarget = target[randomIndex];
            }

        }


        transform.LookAt(enemyTarget.transform.position);
    }

   

}
