using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WarningEye : MonoBehaviour {

    private int numberOfCurrentDetections = 0;
    private EyeUI eyeUI;
    private Image eyeImage;

    void Awake()
    {
        eyeUI = GameObject.FindObjectOfType<EyeUI>();
        if (!eyeUI) { Debug.LogError("No Eye UI on the scene", this); }
        eyeImage = eyeUI.GetComponent<Image>();
    }

    void OnEnable()
    {
        PatrollerAIController.OnPlayerDetected += IncrementDetections;
        PatrollerAIController.OnPlayerLost += DecrementDetections;
        PatrollerAIController.OnPlayerCaught += CatchPlayer;
    }

    void OnDisable()
    {
        PatrollerAIController.OnPlayerDetected -= IncrementDetections;
        PatrollerAIController.OnPlayerLost -= DecrementDetections;
        PatrollerAIController.OnPlayerCaught -= CatchPlayer;
    }

    void IncrementDetections()
    {
        if (numberOfCurrentDetections == 0)
        {
            DrawWarningEye();
        }
        numberOfCurrentDetections++;
    }

    void DecrementDetections()
    {
        numberOfCurrentDetections--;
        if (numberOfCurrentDetections == 0)
        {
            RemoveEye();
        }
    }

    void DrawWarningEye()
    {
        eyeImage.enabled = true;
        eyeUI.SetHalfOpen();
    }

    void RemoveEye()
    {
        eyeImage.enabled = false;
    }

    void CatchPlayer()
    {
        eyeImage.enabled = true;
        eyeUI.SetFullOpen();
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
