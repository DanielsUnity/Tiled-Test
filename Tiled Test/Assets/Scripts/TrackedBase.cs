using UnityEngine;
using System.Collections;

public abstract class TrackedBase : MonoBehaviour {

    public abstract int GetCurrentState(); //We will never want an actual implementation here
    public abstract void SetCurrentState(int state);
}
