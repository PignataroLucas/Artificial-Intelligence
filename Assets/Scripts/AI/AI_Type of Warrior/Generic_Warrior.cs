using UnityEngine;

public class Generic_Warrior : AI 
{

   
    public override void OnUpdate()
    {
        fsm.OnUpdate();
        //Debug.Log(fsm.current);

        if (_genericSO.Class == TypeOfWarriors.Dwarf)
        {
            enemiesInRandius = Physics.OverlapSphere(transform.position, _genericSO.detectionRadius, goblin);
            target = GameObject.FindGameObjectsWithTag("Goblin");           
        }
        else if (_genericSO.Class == TypeOfWarriors.Goblin)
        {
            enemiesInRandius = Physics.OverlapSphere(transform.position, _genericSO.detectionRadius, dwarf);
            target = GameObject.FindGameObjectsWithTag("Dwarf");
        }

        CanDetect();

        if (enemyTarget == null) return;

        transform.LookAt(enemyTarget.transform.position);
    }

    public void CanDetect()
    {
        var n = Detect();        
    }

   



}
