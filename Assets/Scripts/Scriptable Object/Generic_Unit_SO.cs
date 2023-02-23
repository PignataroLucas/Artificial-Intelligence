using UnityEngine;

[CreateAssetMenu(fileName = " New Scriptable Object Unit", menuName = "Stats Unit")]
public class Generic_Unit_SO : ScriptableObject
{
    public int AttackDamage = 0;
    public float AttackRange = 0;
    public int AttackRate = 0;
    public int Life = 0;
    public float Speed = 5;
    public float detectionRadius = 2f;
    public TypeOfWarriors Class;
}
