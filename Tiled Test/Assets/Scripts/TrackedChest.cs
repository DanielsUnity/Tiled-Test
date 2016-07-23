using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InteractableChest))]
public class TrackedChest : TrackedBase {

    public enum ChestState
    {
        Closed = 0,
        Opened = 1
    }

    private ChestState currentState = ChestState.Closed;
    private InteractableChest chest;

    void Awake()
    {
        chest = GetComponent<InteractableChest>();
    }


    public override int GetCurrentState()
    {
        return (int)currentState;
    }


    public override void SetCurrentState(int state) 
    {
        currentState = (ChestState)state;
        chest.OnChangeState(currentState);
    }
}
