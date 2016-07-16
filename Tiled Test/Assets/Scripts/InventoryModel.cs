using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryModel : MonoBehaviour {

    Dictionary<ItemType, int> items = new Dictionary<ItemType, int>();

    public void AddItem(ItemType item)
    {
        AddItem(item, 1);
    }

    public void AddItem(ItemType itemType, int amount)
    {
        if (items.ContainsKey(itemType))
        {
            items[itemType] += amount;
        }
        else
        {
            items.Add(itemType, amount);
        }
        Debug.Log(amount + " " + itemType + " added");
    }
}
