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

        
        //remplazar por consulta directa desde los estados.
        if (this.life <= 0)
        {
            Fsm = new FSM<string>(_deadState);
        }
        
        if (enemyTarget == null) return;

        transform.LookAt(enemyTarget.transform.position);
    }

    

}
