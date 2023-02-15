using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditorInternal;

public class FSM <T>
{
    States<T> _currentState;
    List<States<T>> _states = new List<States<T>>();

    public States<T> current { get { return _currentState; } }

    private bool _init;

    public FSM(States<T> init) 
    {
        _currentState= init;
    }


    public void OnUpdate() 
    {
        if(_init == false)
        {
            _currentState.OnEnter();
            _init = true;
        }
        _currentState.OnUpdate();
    }

    public void OnFixedUpdate()
    {
        _currentState.OnFixedUpdate();
    }

    public void Transition(T input)
    {
        var newState = _currentState.GetState(input);
        if (newState == null) return;
        _currentState.OnSleep();
        _currentState = newState;
        _currentState.OnEnter();
    }


}

public static class State
{
    public const string Idle = "Idle";
    public const string IdleAttack = "IdleAttack";
    public const string Seek = "Seek";
    public const string Attack = "Attack";
    public const string Dead = "Dead";
}
