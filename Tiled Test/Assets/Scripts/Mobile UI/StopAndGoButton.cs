using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StopAndGoButton : ControlButtonBase {

    private State currentState = (State)0;

    private enum State
    {
        Stop,
        Go
    }

    public override void OnClick()
    {
        if (currentState == State.Stop)
        {
            Go();
        }
        else if (currentState == State.Go)
        {
            Stop(); 
        }
    }

    void Go()
    {
        character.SetDirection(character.GetFacingDirection());
        currentState = State.Go;
    }

    public void Stop()
    {
        character.SetDirection(Vector3.zero);
        currentState = State.Stop;
    }
}
