using UnityEngine;
using System.Collections;

public class PlayerBaseController : MonoBehaviour {

	protected PlayerModel playerModel;

	void Start () {
        playerModel = GetComponent<PlayerModel>();
	}


    protected void SetDirection(Vector3 direction)
    {
        playerModel.SetDirection(direction);
    }
}
