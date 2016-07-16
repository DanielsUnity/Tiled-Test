using UnityEngine;
using System.Collections;

public class InteractableChest : InteractableBase {

    public Sprite openedChestSprite;
    public ItemType itemInChest;
    public int amountInChest;

    private bool isOpened = false;


    public override void OnInteractFromBelow(Character character)
    {
        if (!isOpened)
        { 
            GetComponentInChildren<SpriteRenderer>().sprite = openedChestSprite;
            isOpened = true;
            character.inventory.AddItem(itemInChest, amountInChest);
        }
    }

}
