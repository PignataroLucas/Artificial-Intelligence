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
    public static string Idle = "Idle";
    public static string IdleAttack = "IdleAttack";
    public static string Seek = "Seek";
    public static string Attack = "Attack";
    public static string Dead = "Dead";
}
