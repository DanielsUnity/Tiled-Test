using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DirectionButton : ControlButtonBase {

    public Vector3 direction = new Vector3(0,-1,0);

    public override void OnClick()
    {
        character.SetFacingDirection(direction);
    }

    public void InverseDirection()
    {
        character.SetFacingDirection(character.GetFacingDirection() * (-1));
    }
}
