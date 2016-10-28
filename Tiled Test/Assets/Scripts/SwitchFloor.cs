using UnityEngine;
using System.Collections;

public class SwitchFloor : MonoBehaviour {

    public string sortingLayer = "Player Second Floor";

    void OnTriggerExit2D(Collider2D other)
    {
        SpriteRenderer[] spriteRenderers = other.gameObject.GetComponentsInChildren<SpriteRenderer>();
        if (spriteRenderers == null) { Debug.LogError("Couldn't find any sprite renderer in this object's children", other.gameObject); }

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            string currentSortingLayer = spriteRenderers[i].sortingLayerName;
            if (spriteRenderers[i].sortingLayerName != sortingLayer)
            {
                spriteRenderers[i].sortingLayerName = sortingLayer;
            }
        }
    }
}
