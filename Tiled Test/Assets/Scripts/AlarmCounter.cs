using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlarmCounter : MonoBehaviour {

    private int numberOfCurrentDetections = 0;
    private Image counterImage;
    private Text timeLeft;

    void Awake()
    {
        GameObject timeCounter = GameObject.FindGameObjectWithTag("Time Counter");
        if (!timeCounter) { Debug.LogError("No Time Counter on the UI", this); }
        counterImage = timeCounter.GetComponentInChildren<Image>();
        timeLeft = counterImage.GetComponentInChildren<Text>();
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
        Debug.Log("Patroller's detecting player: " + numberOfCurrentDetections);
    }

    void DecrementDetections()
    {
        numberOfCurrentDetections--;
        if (numberOfCurrentDetections == 0)
        {
            StopCounter();
        }
        Debug.Log("Player lost. Patroller's detecting player: " + numberOfCurrentDetections);
    }

    void StartCounter()
    {
        counterImage.enabled = true;
        timeLeft.enabled = true;
        //TODO Actually start the counter, maybe use a corroutine
    }

    void StopCounter()
    {
        timeLeft.enabled = false;
        counterImage.enabled = false;
    }
}
