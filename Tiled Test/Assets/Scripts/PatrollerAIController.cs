using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterBehaviorModel))]
public class PatrollerAIController : CharacterBaseController {

    public GameObject traceStart;
    public float sightDistance = 4;//Sensible value, not too long so the enemy can't "ambush" you when turning
    [Range(10, 60)]
    public float maxSightAngle = 30;

    private CharacterBehaviorModel patroller;

    void Awake()
    {
        patroller = GetComponent<CharacterBehaviorModel>();
    }

    void Update()
    {
        UpdateDirection();
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
            hits.Add(Physics2D.Raycast(traceStart.transform.position, angle, sightDistance, LayerMask.GetMask("Player")));
            Debug.DrawLine(traceStart.transform.position, traceStart.transform.position + ((Vector3)angle * sightDistance), Color.red);
        }
        //TODO Stop on walls

        foreach(RaycastHit2D hit in hits)
        {
            if (hit)
            {
                Debug.Log("Player hit, " + Time.realtimeSinceStartup);
                break;
            }
        }
  
    }

    private void UpdateDirection()
    {
        //For now we'll debug by using the controller, later it has to patrol automatically
        float horizontal = Input.GetAxis("Keyboard Horizontal");
        float vertical = Input.GetAxis("Keyboard Vertical");

        float xDirection;
        float yDirection;
        float xAbs = Mathf.Abs(horizontal);
        float yAbs = Mathf.Abs(vertical);

        if (xAbs != 0) { xDirection = horizontal / xAbs; } else { xDirection = 0; }
        if (yAbs != 0) { yDirection = vertical / yAbs; } else { yDirection = 0; }

        Vector3 movementVector = new Vector3(xDirection, yDirection, 0);
        movementVector.Normalize();

        SetDirection(movementVector);
    }
}
