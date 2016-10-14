using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StopAndGoButton : MonoBehaviour {

    private Button button;
    private CharacterBehaviorModel character;
    private State currentState = (State)0;

    private enum State
    {
        Stop,
        Go
    }

    void Awake()
    {
        button = GetComponent<Button>();
        if (!button) { Debug.LogError("Button not found!", this); }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!player)
        {
            Debug.LogError("Player not found!", this);
        }
        else
        {
            character = player.GetComponent<CharacterBehaviorModel>();
        }
    }
	
	
	void Update ()
    {
        //In case player dies, get the component again
        if (!character)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (!player)
            {
                Debug.LogError("Player not found!", this);
            }
            else
            {
                character = player.GetComponent<CharacterBehaviorModel>();
            }
        }
    }

    public void OnClick()
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

    void Stop()
    {
        character.SetDirection(Vector3.zero);
        currentState = State.Stop;
    }
}
