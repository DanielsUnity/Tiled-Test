using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlButtonBase : MonoBehaviour {

    protected CharacterBehaviorModel character;
    protected CharacterInteractionModel interactionModel;
    protected Button button;
 
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
            interactionModel = player.GetComponent<CharacterInteractionModel>();
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

    public virtual void OnClick()
    {
        Debug.Log("This is meant to be overridden");
    } 
}
