using UnityEngine;
using System.Collections;

public class CharacterBaseController : MonoBehaviour {

	protected CharacterBehaviorModel characterModel;
    protected CharacterInteractionModel interactionModel;

	void Start () {
        characterModel = GetComponent<CharacterBehaviorModel>();
        interactionModel = GetComponent<CharacterInteractionModel>();
	}


    protected void SetDirection(Vector3 direction)
    {
        if (characterModel == null) { return; }
        Debug.Log("Moving towards " + direction);
        characterModel.SetDirection(direction);
    }

    protected void OnActionPressed()
    {
        if (interactionModel == null) { return; }
        interactionModel.OnInteract();
    }
}
