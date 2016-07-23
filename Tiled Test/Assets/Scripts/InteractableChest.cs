using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TrackedChest))]
public class InteractableChest : InteractableBase {

    public Sprite openedChestSprite;
    public ItemType itemInChest;
    public int amountInChest;

    private TrackedChest trackedChest;

    void Awake()
    {
        trackedChest = GetComponent<TrackedChest>();
    }

    public override void OnInteractFromBelow(Character character)
    {
        if (trackedChest.GetCurrentState() == (int)TrackedChest.ChestState.Closed)
        { 
            trackedChest.SetCurrentState((int)TrackedChest.ChestState.Opened);
            character.inventory.AddItem(itemInChest, amountInChest);
        }
    }

    public void OnChangeState(TrackedChest.ChestState newState)
    {
        if (newState == TrackedChest.ChestState.Opened)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = openedChestSprite;
        }
    }

}
