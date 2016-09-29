using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class FollowerCamera : MonoBehaviour {

    private Tiled2Unity.TiledMap stage;
    private Camera thisCamera;
    private Transform objectToFollow;
    private float xBoundary;
    private float yUpperBoundary;
    private float yLowerBoundary;
    private float UIHeight = 0;
    private float cameraPlayerOffset = 0;

    void Awake()
    {
        stage = FindObjectOfType<Tiled2Unity.TiledMap>();
        if (!stage) { Debug.LogError("Stage not generated with Tiled2Unity. This type of camera needs it"); }
        thisCamera = GetComponent<Camera>();
        objectToFollow = GameObject.FindGameObjectWithTag("Player").transform;//TODO Check what happens if we try to spawn the character
        if (!objectToFollow) { Debug.LogWarning("Camera hasn't found the player at awake time"); }

        GameObject mobileUI = GameObject.FindGameObjectWithTag("Mobile UI");
        if (!mobileUI)
        {
            Debug.LogWarning("No Mobile UI");
        }
        else
        {
            float UIScreenVerticalPercentage = mobileUI.GetComponent<RectTransform>().anchorMax.y;
            UIHeight = thisCamera.orthographicSize * 2 * UIScreenVerticalPercentage;
        }
    }

	void Start () {
        float gameplayViewportCenter = (thisCamera.orthographicSize * 2 - UIHeight) / 2;
        cameraPlayerOffset = thisCamera.orthographicSize - gameplayViewportCenter;

        yUpperBoundary = stage.NumTilesHigh/2 - thisCamera.orthographicSize; //this presupposes that every tile is one unit in size
        yLowerBoundary = -stage.NumTilesHigh/2 + thisCamera.orthographicSize - UIHeight;

        xBoundary = stage.NumTilesWide/2 - thisCamera.orthographicSize * thisCamera.aspect;
    }
	
	void Update () {
        float newCameraCenter = objectToFollow.position.y - cameraPlayerOffset;
        transform.position = new Vector3(objectToFollow.position.x, newCameraCenter, transform.position.z);

        float currentXPosition = transform.position.x;
        float currentYPosition = transform.position.y;

        currentXPosition = Mathf.Clamp(currentXPosition,-xBoundary,xBoundary);
        currentYPosition = Mathf.Clamp(currentYPosition,yLowerBoundary,yUpperBoundary);

        transform.position = new Vector3(currentXPosition,currentYPosition,transform.position.z);
    }
}
