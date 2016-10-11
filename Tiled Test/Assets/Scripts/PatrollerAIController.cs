using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterBehaviorModel))]
[RequireComponent(typeof(PatrollerStateManager))]
public class PatrollerAIController : CharacterBaseController {

    public GameObject traceStart;
    public float sightDistance = 4;//Sensible value, not too long so the enemy can't "ambush" you when turning

    [Range(10, 60)]
    public float maxSightAngle = 30;

    public BeaconBase[] beacons;


    private CharacterBehaviorModel patroller;
    private PatrollerStateManager stateManager;

    private BeaconBase nextBeacon;
    private int beaconIndex = 0;
    private int numberOfBeacons;

    private bool isInspecting = false;



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

        List<Vector2> sightAngles = new List<Vector2>();
        float deltaAngle = maxSightAngle * 2 / 8; // 9 Raycasts

        for (float angle = maxSightAngle; angle >= -maxSightAngle; angle -= deltaAngle)
        {
            sightAngles.Add(Quaternion.Euler(0, 0, angle) * facingDirection);
        }

        List<RaycastHit2D> hits = new List<RaycastHit2D>();

        foreach (Vector2 angle in sightAngles)
        {
            hits.Add(Physics2D.Raycast(traceStart.transform.position, angle, sightDistance, LayerMask.GetMask("Player","Block Raycast")));
            Debug.DrawLine(traceStart.transform.position, traceStart.transform.position + ((Vector3)angle * sightDistance), Color.red);
        }

        foreach(RaycastHit2D hit in hits)
        {
            if (hit)
            {
                if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
                {
                    if (stateManager.GetCurrentState() != PatrollerStateManager.State.EnemyDetected)
                    {
                        stateManager.SetCurrentState(PatrollerStateManager.State.EnemyDetected);
                    }
                    return;
                }
            }
        }

        if (stateManager.GetCurrentState() != PatrollerStateManager.State.Patrol)
        {
            stateManager.SetCurrentState(PatrollerStateManager.State.Patrol);
        }

    }


    public void EnemyDetectedBehavior()
    {
        //TODO Provisional behavior
        if (numberOfBeacons > 0)
        { 
            SetDirection(characterModel.GetFacingDirection());
        }
        //Start counter and extend patrollers' sight
    }

    public void PatrolBehavior()
    {
        if (!isInspecting)
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
