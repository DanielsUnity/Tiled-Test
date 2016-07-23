using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class OrderInLayerManager : MonoBehaviour {

    private int halfNumberOfLayers = 100;

    private SpriteRenderer sprite;
    private Camera mainCamera;

	void Awake () {
        sprite = GetComponent<SpriteRenderer>();
        mainCamera = FindObjectOfType<Camera>();
	}
	
	void Update () {
        float relativeHeight = GetComponentInParent<Transform>().position.y - mainCamera.transform.position.y;

        if (Mathf.Abs(relativeHeight) > 2*mainCamera.orthographicSize) //If the object is outside the camera range we set the order to 0 (given a margin of twice the camera size)
        {
            sprite.sortingOrder = 0;
            return;
        }

        int orderInLayer = Mathf.RoundToInt(relativeHeight * halfNumberOfLayers / mainCamera.orthographicSize);
        sprite.sortingOrder = - orderInLayer;
	}
}
