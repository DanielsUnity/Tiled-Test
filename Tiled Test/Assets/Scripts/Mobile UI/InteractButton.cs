using UnityEngine;
using System.Collections;

public class InteractButton : ControlButtonBase {

    public override void OnClick()
    {
        interactionModel.OnInteract();
    }
}
