using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DialogManager))]
public class InteractableSign : InteractableBase {

    public string alternateMessage = "You cannot read anything from here";

    private DialogManager dialogManager;

    void Awake()
    {
        dialogManager = GetComponent<DialogManager>();
    }

    public override void OnInteractFromBelow(Character character)
    {
        //Aside from the dialog box we could trigger some event, at most, otherwise this line should always go alone
        dialogManager.Manage();
    }

    public override void OnInteractFromAbove(Character character)
    {
        dialogManager.WriteAlternateMessage(alternateMessage);
    }
}
