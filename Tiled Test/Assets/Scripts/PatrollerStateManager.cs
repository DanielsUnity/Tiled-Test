using UnityEngine;
using System.Collections;
using System;

public class PatrollerStateManager : StateManager {

    //Next 3 variables are used for every class extending StateManager
    private State currentState = (State)0;
    private string[] stateNames = System.Enum.GetNames(typeof(State));
    private int stateCount = System.Enum.GetValues(typeof(State)).Length;

    private PatrollerAIController patrollerAIController;

    public enum State
    {
        Patrol,
        EnemyDetected
        //Optional "Alert"
    }

    private void Awake()
    {
        patrollerAIController = GetComponent<PatrollerAIController>();
        if (!patrollerAIController) { Debug.LogError("Can't find PatrollerAIController", this); }
    }

    public void OnDefault()
    {
        OnPatrol();
    }

    public override void DefaultBehavior()
    {
        PatrolBehavior();
    }

    public State GetCurrentState()
    {
        return currentState;
    }

    public override int GetCurrentStateInteger()
    {
        return (int)currentState;
    }

    public override string GetCurrentStateString()
    {
        return stateNames[(int)currentState];
    }

    public void SetCurrentState(State state)
    {
        currentState = state;
        OnChangeState();
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

    void OnChangeState()
    {
        if (currentState == State.Patrol)
        {
            OnPatrol();
        }
        else if(currentState == State.EnemyDetected)
        {
            OnEnemyDetected();
        }
        else
        {
            OnDefault();
        }
    }

    void OnEnemyDetected()
    {
        Debug.Log("Enemy detected");
    }

    void OnPatrol()
    {
        Debug.Log("Start patrolling");
    }

    void Update()
    {
        ExecuteStateBehavior();
    }

    public void ExecuteStateBehavior()
    {
        if (currentState == State.Patrol)
        {
            PatrolBehavior();
        }
        else if (currentState == State.EnemyDetected)
        {
            EnemyDetectedBehavior();
        }
        else
        {
            DefaultBehavior();
        }
    }

    private void PatrolBehavior()
    {
        patrollerAIController.PatrolBehavior();
    }

    private void EnemyDetectedBehavior()
    {
        patrollerAIController.EnemyDetectedBehavior();
    }
}
