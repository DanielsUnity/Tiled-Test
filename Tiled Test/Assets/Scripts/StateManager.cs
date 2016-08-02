using UnityEngine;
using System.Collections;

public abstract class StateManager : MonoBehaviour {

    public abstract void SetCurrentState(string state);//externally used in DialogManager 

    public abstract void SetCurrentState(int state);

    public abstract int GetCurrentStateInteger();//externally used in TrackedObject
}
