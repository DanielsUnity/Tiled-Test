using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlarmCounter : MonoBehaviour {

    public int alertTime = 3;

    private int numberOfCurrentDetections = 0;
    private Image counterImage;
    private Text timeLeft;
    private int timer;
    private IEnumerator countdownCoroutine;

    void Awake()
    {
        GameObject timeCounter = GameObject.FindGameObjectWithTag("Time Counter");
        if (!timeCounter) { Debug.LogError("No Time Counter on the UI", this); }
        counterImage = timeCounter.GetComponentInChildren<Image>();
        timeLeft = counterImage.GetComponentInChildren<Text>();
    }

    void Start()
    {
        timer = alertTime;
        countdownCoroutine = Countdown();
    }

    void OnEnable()
    {
        PatrollerAIController.OnPlayerDetected += IncrementDetections;
        PatrollerAIController.OnPlayerLost += DecrementDetections;
    }

    void OnDisable()
    {
        PatrollerAIController.OnPlayerDetected -= IncrementDetections;
        PatrollerAIController.OnPlayerLost -= DecrementDetections;
    }

    void IncrementDetections()
    {
        if (numberOfCurrentDetections == 0)
        {
            StartCounter();
        }
        numberOfCurrentDetections++;
    }

    void DecrementDetections()
    {
        numberOfCurrentDetections--;
        if (numberOfCurrentDetections == 0)
        {
            StopCounter();
        }
    }

    void StartCounter()
    {
        counterImage.enabled = true;
        timeLeft.enabled = true;
        StartCoroutine(countdownCoroutine);
    }

    void StopCounter()
    {
        timeLeft.enabled = false;
        counterImage.enabled = false;
        StopCoroutine(countdownCoroutine);
        countdownCoroutine = Countdown();
        timer = alertTime;
    }

    IEnumerator Countdown()
    {
        do
        {
            timeLeft.text = timer.ToString();
            if (timer == 0)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (!player) { Debug.LogError("Player not found", this); }
                player.GetComponent<CharacterBehaviorModel>().Freeze();

                GameObject portal = GameObject.FindGameObjectWithTag("Portal");
                if (!portal) { Debug.LogError("Portal not found", this); }
                portal.GetComponent<SpriteRenderer>().enabled = true;
                Animator animator = portal.GetComponent<Animator>();
                animator.enabled = true;
                animator.SetTrigger("Enable Trigger");

                break;
            }
            yield return new WaitForSeconds(1);//if stop counter is executed while in here the loop won't stop
            timer--;
        } while (true);
    }
}
