using UnityEngine;
using System.Collections;
using System;

public class NPCStateManager : StateManager
{
    public Sprite facingDown;

    //Next 3 variables are used for every class extending StateManager
    private State currentState = (State)0;
    private string[] stateNames = System.Enum.GetNames(typeof(State));
    private int stateCount = System.Enum.GetValues(typeof(State)).Length;

    private SpriteRenderer spriteRenderer;

    public enum State
    {
        Neutral,
        Welcoming,
        Panic,
        Kidding
    }

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();//Don't do that if the character may have more than one sprite renderer (maybe carrying an object)
        if (!spriteRenderer) { Debug.LogError("Sprite renderer not found in child!", this); }
    }

    public void OnDefault()
    {
        OnNeutral();
    }

    public override void  DefaultBehavior()
    {
        //Set this to an already defined behavior, useful for different conversation states with the same behavior
        NeutralBehavior();
    }

    public override void SetCurrentState(string state)
    {
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
        currentState = state;
        OnChangeState();
    }

    public override int GetCurrentStateInteger()
    {
        return (int)currentState;
    }

    public override string GetCurrentStateString()
    {
        return stateNames[(int)currentState];
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
        else
        {
            OnDefault();
        }
    }

    private void OnNeutral()
    {
        
    }

    void FixedUpdate()
    {
        ExecuteStateBehavior();
    } 

    public void ExecuteStateBehavior()
    {
        if (currentState == State.Neutral)
        {
            NeutralBehavior();
        }
        else
        {
            DefaultBehavior();
        }
    }

    private void NeutralBehavior()
    {
        spriteRenderer.sprite = facingDown;
    }
}
