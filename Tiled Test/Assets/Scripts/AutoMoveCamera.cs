using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class AutoMoveCamera : MonoBehaviour {

    public float velocity = 1;

    private Tiled2Unity.TiledMap stage;
    private Camera thisCamera;
    private Transform objectToFollow;
    private float xBoundary;
    private float yUpperBoundary;
    private float yLowerBoundary;
    private float UIHeight = 0;
    private float cameraPlayerOffset = 0;
    private float exportScale = 0.03125f;//32 pixels per unit by default

    private bool isColliding = false;
    private Dictionary<GameObject, Collision2D> characterCollisions = new Dictionary<GameObject, Collision2D>();

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

    void Start()
    {
        exportScale = stage.ExportScale;

        float gameplayViewportCenter = (thisCamera.orthographicSize * 2 - UIHeight) / 2;
        cameraPlayerOffset = thisCamera.orthographicSize - gameplayViewportCenter;

        yUpperBoundary = (stage.MapHeightInPixels / 2) * exportScale - thisCamera.orthographicSize;
        yLowerBoundary = -(stage.MapHeightInPixels / 2) * exportScale + thisCamera.orthographicSize - UIHeight;

        xBoundary = (stage.MapWidthInPixels / 2) * exportScale - thisCamera.orthographicSize * thisCamera.aspect;

        //Clamp initial position

        float currentXPosition = transform.position.x;
        float currentYPosition = transform.position.y;

        currentXPosition = Mathf.Clamp(currentXPosition, -xBoundary, xBoundary);
        currentYPosition = Mathf.Clamp(currentYPosition, yLowerBoundary, yUpperBoundary);

        Vector3 clampedPosition = new Vector3(currentXPosition, currentYPosition, transform.position.z);
        transform.position = clampedPosition;
    }

    void Update()
    {
        float newY = transform.position.y + velocity * Time.deltaTime;//+velocity*deltaTime
        Vector3 newPosition = new Vector3(objectToFollow.position.x, newY, transform.position.z);
        transform.position = newPosition;

        float currentXPosition = transform.position.x;
        float currentYPosition = transform.position.y;

        currentXPosition = Mathf.Clamp(currentXPosition, -xBoundary, xBoundary);
        currentYPosition = Mathf.Clamp(currentYPosition, yLowerBoundary, yUpperBoundary);

        Vector3 clampedPosition = new Vector3(currentXPosition, currentYPosition, transform.position.z);
        transform.position = clampedPosition;
    }

    void OnEnable()
    {
        CharacterInteractionModel.OnPlayerColliding += IncrementPlayerCollisions;
        CharacterInteractionModel.OnPlayerLeavingCollider += DecrementPlayerCollisions;
    }

    void OnDisable()
    {
        CharacterInteractionModel.OnPlayerColliding -= IncrementPlayerCollisions;
        CharacterInteractionModel.OnPlayerLeavingCollider -= DecrementPlayerCollisions;
    }

    void IncrementPlayerCollisions(Collision2D collision)
    {
        if (collision.contacts[0].normal.y < -0.7)//If extended to fit more camera directions this should be changed
        {
            characterCollisions.Add(collision.gameObject, collision);
            Debug.Log("Count: " + characterCollisions.Count);
        }
    }

    void DecrementPlayerCollisions(Collision2D collision)
    {
        characterCollisions.Remove(collision.gameObject);
        Debug.Log("Count: " + characterCollisions.Count);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isColliding = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isColliding = false;
        }
    }

    void FixedUpdate()
    {
        if (isColliding && characterCollisions.Count > 0)
        {
            velocity = 0;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (!player) { Debug.LogError("Player not found", this); }
            player.GetComponent<CharacterBehaviorModel>().Freeze();

            GameObject portal = GameObject.FindGameObjectWithTag("Portal");
            if (!portal) { Debug.LogError("Portal not found", this); }
            portal.GetComponent<SpriteRenderer>().enabled = true;
            Animator animator = portal.GetComponent<Animator>();
            animator.enabled = true;
            animator.SetTrigger("Enable Trigger");
        }
    }
}
