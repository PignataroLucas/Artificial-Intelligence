using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class States <T>
{
    Dictionary <T,States<T>> _actions = new Dictionary <T,States<T>>();

    public virtual void OnEnter() { }

    public virtual void OnUpdate() { }

    public virtual void OnFixedUpdate() { }

    public virtual void OnLateUpdate() { }

    public virtual void OnSleep() { }


    public void SetTransition(T input,States<T> state)
    {
        if (!_actions.ContainsKey(input)) _actions.Add(input, state);
    }

    public States<T> GetState(T input)
    {

        if (_actions.ContainsKey(input)) return _actions[input];
        else return null;
    }


}
