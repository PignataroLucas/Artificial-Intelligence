using UnityEngine;
using System.Collections;

public class IdleState <T> : States<T>
{
    private AI _ai;

    public IdleState(AI ai)
    {
        _ai = ai;
    }

    public override void OnEnter()
    {
        SetTransitionAnim();
    }
    public void SetTransitionAnim()
    {         
        _ai.animator.SetInteger("idleType", Random.Range(0, 4));
        _ai.animator.SetBool("canIdle", true);        
    }
}
