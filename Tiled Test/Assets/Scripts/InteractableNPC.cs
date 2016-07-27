using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DialogManager))]
public class InteractableNPC : InteractableBase
{
    private DialogManager dialogManager;

    void Awake()
    {
        dialogManager = GetComponent<DialogManager>();
    }

    public override void OnInteractFromBelow(Character character)
    {
        //Face that way
        dialogManager.Manage();
    }

    public override void OnInteractFromAbove(Character character)
    {
        dialogManager.Manage();
    }
}