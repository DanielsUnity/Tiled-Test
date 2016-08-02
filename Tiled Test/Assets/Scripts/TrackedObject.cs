using UnityEngine;
using System.Collections;

[RequireComponent(typeof(StateManager))]
public class TrackedObject : MonoBehaviour {//hacerla no abstracta e igual para todos

    private StateManager stateManager;

    void Awake()
    {
        stateManager = GetComponent<StateManager>();
    }

    public int GetCurrentState()
    {
        return stateManager.GetCurrentStateInteger();
    }

    public void SetCurrentState(int state)
    {
        stateManager.SetCurrentState(state);
    }
}
