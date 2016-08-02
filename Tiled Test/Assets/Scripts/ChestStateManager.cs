using UnityEngine;
using System.Collections;

public class ChestStateManager : StateManager
{

    public Sprite openedChestSprite;

    private State currentState = (State)0;
    private string[] stateNames = System.Enum.GetNames(typeof(State));
    private int stateCount = System.Enum.GetValues(typeof(State)).Length;

    public enum State
    {
        Closed,
        Opened
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
        if (currentState == State.Opened)
        {
            OnOpened();
        }
        //Don't need anything on OnClosed

    }

    void OnOpened()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = openedChestSprite;
    }
}
