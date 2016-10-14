using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DirectionButton : MonoBehaviour {

    public Vector3 direction = new Vector3(0,-1,0);

    private CharacterBehaviorModel character;
    private Button button;

    //Same two methods as in StopAndGoButton, hmmmm, rethink if it happens a third time, for now it's ok 
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

    void Update()
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
        character.SetFacingDirection(direction);
    }

    public void InverseDirection()
    {
        character.SetFacingDirection(character.GetFacingDirection() * (-1));
    }
}
