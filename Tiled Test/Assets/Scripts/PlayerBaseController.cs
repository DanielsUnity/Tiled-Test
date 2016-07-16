using UnityEngine;
using System.Collections;

public class PlayerBaseController : MonoBehaviour {

	protected PlayerModel playerModel;
    protected CharacterInteractionModel interactionModel;

	void Start () {
        playerModel = GetComponent<PlayerModel>();
        interactionModel = GetComponent<CharacterInteractionModel>();
	}


    protected void SetDirection(Vector3 direction)
    {
        if (playerModel == null) { return; }
        playerModel.SetDirection(direction);
    }

    protected void OnActionPressed()
    {
        if (interactionModel == null) { return; }
        //TODO Finish OnActionPressed
        interactionModel.OnInteract();
    }
}
