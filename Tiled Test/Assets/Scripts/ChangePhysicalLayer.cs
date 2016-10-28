using UnityEngine;
using System.Collections;

public class ChangePhysicalLayer : MonoBehaviour {

    public string physicalLayer = "Player";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer(physicalLayer))
        {
            other.gameObject.layer = LayerMask.NameToLayer(physicalLayer);
        }
    }
}
