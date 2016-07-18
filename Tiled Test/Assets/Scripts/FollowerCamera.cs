using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class FollowerCamera : MonoBehaviour {

    private Tiled2Unity.TiledMap stage;
    private Camera thisCamera;
    private Transform objectToFollow;
    private float xBoundary;
    private float yBoundary;

    void Awake()
    {
        stage = FindObjectOfType<Tiled2Unity.TiledMap>();
        if (!stage) { Debug.LogError("Stage not generated with Tiled2Unity. This type of camera needs it"); }
        thisCamera = GetComponent<Camera>();
        objectToFollow = GameObject.FindGameObjectWithTag("Player").transform;//TODO Check what happens if we try to spawn the character
        if (!objectToFollow) { Debug.LogWarning("Camera hasn't found the player at awake time"); }
    }

	void Start () {
        yBoundary = stage.NumTilesHigh/2 - thisCamera.orthographicSize; //this presupposes that every tile is one unit in size
        xBoundary = stage.NumTilesWide/2 - thisCamera.orthographicSize * thisCamera.aspect;
    }
	
	void Update () {
        transform.position = new Vector3(objectToFollow.position.x,objectToFollow.position.y,transform.position.z);
        float currentXPosition = transform.position.x;
        float currentYPosition = transform.position.y;
        currentXPosition = Mathf.Clamp(currentXPosition,-xBoundary,xBoundary);
        currentYPosition = Mathf.Clamp(currentYPosition,-yBoundary,yBoundary);
        transform.position = new Vector3(currentXPosition,currentYPosition,transform.position.z);
    }
}
