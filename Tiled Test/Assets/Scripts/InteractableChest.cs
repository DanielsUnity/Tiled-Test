using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ChestStateManager))]
public class InteractableChest : InteractableBase {

    public ItemType itemInChest;
    public int amountInChest;

    private ChestStateManager chestStateManager;

    void Awake()
    {
        chestStateManager = GetComponent<ChestStateManager>();
    }

    public override void OnInteractFromBelow(Character character)
    {
        if (chestStateManager.GetCurrentState() == ChestStateManager.State.Closed)
        { 
            chestStateManager.SetCurrentState(ChestStateManager.State.Opened);
            character.inventory.AddItem(itemInChest, amountInChest);
        }
    }
}
