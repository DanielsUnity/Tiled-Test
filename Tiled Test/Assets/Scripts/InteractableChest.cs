using UnityEngine;
using System.Collections;

public class InteractableChest : InteractableBase {

    public override void OnInteractFromLeftSide()
    {
        Debug.Log("Interacting with chest from the left side");
    }

    public override void OnInteractFromRightSide()
    {
        Debug.Log("Interacting with chest from the right side");
    }

    public override void OnInteractFromAbove()
    {
        Debug.Log("Interacting with chest from above");
    }

    public override void OnInteractFromBelow()
    {
        Debug.Log("Interacting with chest from below");
    }

}
