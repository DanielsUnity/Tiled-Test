﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterBehaviorModel))]
[RequireComponent(typeof(PatrollerStateManager))]
public class PatrollerAIController : CharacterBaseController {

    public GameObject traceStart;
    public float warningSightDistance = 4;//Sensible value, not too long so the enemy can't "ambush" you when turning
    public float alertSightDistance = 2.5f;
    [Range(1, 2)]
    public float warningSightMultiplier = 1.5f;

    [Range(10, 60)]
    public float maxWarningSightAngle = 30;
    [Range(10, 60)]
    public float maxAlertSightAngle = 30;

    public float surprisedTime = 1;

    public BeaconBase[] beacons;

    public delegate void PatrollerDelegate();
    public static event PatrollerDelegate OnPlayerDetected;
    public static event PatrollerDelegate OnPlayerLost;
    public static event PatrollerDelegate OnPlayerCaught;


    private CharacterBehaviorModel patroller;
    private PatrollerStateManager stateManager;

    private BeaconBase nextBeacon;
    private int beaconIndex = 0;
    private int numberOfBeacons;

    private bool isInspecting = false;
    private bool isSurprised = false;



    void Awake()
    {
        patroller = GetComponent<CharacterBehaviorModel>();
        stateManager = GetComponent<PatrollerStateManager>();
        
        numberOfBeacons = beacons.Length;
        if (numberOfBeacons > 0) { nextBeacon = beacons[0]; } else { isInspecting = true; }
    }

    void Update()
    {
        LookForIntruders();
    }

    private void LookForIntruders()
    {
        Vector2 facingDirection = patroller.GetFacingDirection();

        List<Vector2> warningSightAngles = new List<Vector2>();
        float warningDeltaAngle = maxWarningSightAngle * 2 / 8; // 9 Raycasts

        for (float angle = maxWarningSightAngle; angle >= -maxWarningSightAngle; angle -= warningDeltaAngle)
        {
            warningSightAngles.Add(Quaternion.Euler(0, 0, angle) * facingDirection);
        }

        List<Vector2> alertSightAngles = new List<Vector2>();
        float alertDeltaAngle = maxAlertSightAngle * 2 / 8;

        for (float angle = maxAlertSightAngle; angle >= -maxAlertSightAngle; angle -= alertDeltaAngle)
        {
            alertSightAngles.Add(Quaternion.Euler(0, 0, angle) * facingDirection);
        }

        List<RaycastHit2D> warningHits = new List<RaycastHit2D>();

        foreach (Vector2 angle in warningSightAngles)
        {
            warningHits.Add(Physics2D.Raycast(traceStart.transform.position, angle, warningSightDistance, LayerMask.GetMask("Player","Block Raycast")));
            Debug.DrawLine(traceStart.transform.position, traceStart.transform.position + ((Vector3)angle * warningSightDistance), Color.blue);
        }

        List<RaycastHit2D> alertHits = new List<RaycastHit2D>();

        foreach (Vector2 angle in alertSightAngles)
        {
            alertHits.Add(Physics2D.Raycast(traceStart.transform.position, angle, alertSightDistance, LayerMask.GetMask("Player", "Block Raycast")));
            Debug.DrawLine(traceStart.transform.position, traceStart.transform.position + ((Vector3)angle * alertSightDistance), Color.red);
        }

        foreach (RaycastHit2D hit in alertHits)
        {
            if (hit)
            {
                if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
                {
                    CancelInvoke();
                    isSurprised = true;
                    SetDirection(Vector3.zero);
                    if (OnPlayerDetected != null)
                    {
                        OnPlayerCaught();
                    }
                    return;
                }
            }
        }

        foreach (RaycastHit2D hit in warningHits)
        {
            if (hit)
            {
                if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
                {
                    if (stateManager.GetCurrentState() != PatrollerStateManager.State.EnemyDetected)
                    {
                        isSurprised = true;
                        SetDirection(Vector3.zero);
                        stateManager.SetCurrentState(PatrollerStateManager.State.EnemyDetected);
                        //Schedule restart patrolling
                        Invoke("FinishSurprise", surprisedTime);

                        //Throw enemy detected event
                        if (OnPlayerDetected != null)
                        {
                            OnPlayerDetected();
                        }
                        warningSightDistance *= warningSightMultiplier;
                    }
                    return;
                }
            }
        }

        

        if (stateManager.GetCurrentState() != PatrollerStateManager.State.Patrol)
        {
            RestartPatrolling();
        }

    }

    private void RestartPatrolling()
    {
        stateManager.SetCurrentState(PatrollerStateManager.State.Patrol);
        //Throw enemy lost event
        if (OnPlayerLost != null)
        {
            OnPlayerLost();
        }
        warningSightDistance /= warningSightMultiplier;
    }

    public void EnemyDetectedBehavior()
    {
        if (!isSurprised)
        {
            PatrolBehavior();
        }
    }

    void FinishSurprise()
    {
        isSurprised = false;
        Debug.Log("Invoking finish surprise");
    }

    public void PatrolBehavior()
    {
        if (!isInspecting && !isSurprised)
        {
            Vector2 directionVector = nextBeacon.transform.position - transform.position;
            if (directionVector.sqrMagnitude > Mathf.Pow(1 / patroller.speed, 2))
            {
                NormalizeAndSetDirection(directionVector.x, directionVector.y);
            }
            else
            {
                SetDirection(directionVector / patroller.speed);
                BeaconBase currentBeacon = nextBeacon;
                SetNextBeacon();
                currentBeacon.ArrivedToBeacon(this);
            }
        }
    }

    void NormalizeAndSetDirection(float xDirection, float yDirection)
    {
        Vector3 movementVector = new Vector3(xDirection, yDirection, 0);
        movementVector.Normalize();
        SetDirection(movementVector);
    }

    public void SetNextBeacon()
    {
        beaconIndex++;
        beaconIndex %= numberOfBeacons;
        nextBeacon = beacons[beaconIndex];
    }

    public IEnumerator WaitInPlace(Vector3 facingDirection, float seconds)
    {
        isInspecting = true;
        SetDirection(Vector3.zero);
        characterModel.SetFacingDirection(facingDirection);
        
        yield return new WaitForSeconds(seconds);
        isInspecting = false;

    }

    public void FaceDirection(Vector3 direction)
    {
        characterModel.SetFacingDirection(direction);
    }
}
