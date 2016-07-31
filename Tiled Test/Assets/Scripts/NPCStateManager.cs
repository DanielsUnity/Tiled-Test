using UnityEngine;
using System.Collections;
using System;

public class NPCStateManager : StateManager
{

    private State currentState;
    private string[] stateNames = System.Enum.GetNames(typeof(State));
    private int stateCount = System.Enum.GetValues(typeof(State)).Length;

    public enum State
    {
        Neutral,
        Welcoming,
        Panic
    }


    public override void SetCurrentState(string state)
    {
        //Probably work for a super class
        foreach (string name in stateNames) 
        {
            if (name == state)
            {
                currentState = (State)System.Enum.Parse(typeof(State), state);
                OnChangeState();
                return;
            }
        }
        Debug.LogWarning("The State trying to access does not exist: " + state);
    }

    public override void SetCurrentState(int state)
    {
        //Probably work for a super class
        if (state < stateCount && state >= 0)
        {
            currentState = (State)state;
            OnChangeState();
        }
        else
        {
            Debug.LogWarning("The State trying to access does not exist: " + state);
        }
    }

    public void SetCurrentState(State state)
    {
        //Probably work for a super class
        currentState = state;
        OnChangeState();
    }

    public override int GetCurrentStateInteger()
    {
        return (int)currentState;
    }

    public State GetCurrentState()
    {
        return currentState;
    }

    void OnChangeState()
    {
        if (currentState == State.Neutral)
        {
            OnNeutral();
        }
        if (currentState == State.Panic)
        {
            OnPanic();
        }
        if (currentState == State.Welcoming)
        {
            OnWelcoming();
        }
    }

    private void OnWelcoming()
    {
        Debug.Log("State changed to \"welcoming\"");
    }

    private void OnPanic()
    {
        Debug.Log("State changed to \"panic\"");
    }

    private void OnNeutral()
    {
        Debug.Log("State changed to \"neutral\"");
    }

    public void ExecuteStateBehavior()
    {
        if (currentState == State.Neutral)
        {
            NeutralBehavior();
        }
        if (currentState == State.Panic)
        {
            PanicBehavior();
        }
        if (currentState == State.Welcoming)
        {
            WelcomingBehavior();
        }
    }

    private void WelcomingBehavior()
    {
        
    }

    private void PanicBehavior()
    {
        
    }

    private void NeutralBehavior()
    {
        
    }
}
