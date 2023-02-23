using UnityEngine;

public class Generic_Warrior : AI 
{
    public override void OnUpdate()
    {
        Fsm.OnUpdate();
        //Debug.Log(fsm.current);
        if (genericSo.Class == TypeOfWarriors.Dwarf)
        {
            enemiesInRandius = Physics.OverlapSphere(transform.position, genericSo.detectionRadius, goblin);
            target = GameObject.FindGameObjectsWithTag("Goblin");           
        }
        else if (genericSo.Class == TypeOfWarriors.Goblin)
        {
            enemiesInRandius = Physics.OverlapSphere(transform.position, genericSo.detectionRadius, dwarf);
            target = GameObject.FindGameObjectsWithTag("Dwarf");
        }
        Debug.Log(UnitStat.Life);
        if (enemyTarget != null)
        {
            transform.LookAt(enemyTarget.transform.position);
            //return;
        }
        
    }
}
