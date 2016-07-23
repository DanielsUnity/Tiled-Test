using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public static Character playerInstance = null;

    public CharacterInteractionModel interactionModel;
    public CharacterBehaviorModel characterModel;
    public CharacterView characterView;
    public InventoryModel inventory;

    void Awake()
    {
        if (tag == "Player")
        {
            if (playerInstance == null)
            {
                playerInstance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
            DontDestroyOnLoad(this);
        }
    }
}
