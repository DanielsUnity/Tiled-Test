using UnityEngine;
using System.Collections;
using System;

public class CharacterBehaviorModel : MonoBehaviour {

    public float speed = 5f;
    public Vector3 facingDirection = Vector3.down;

    private Rigidbody2D playerBody;
    private Vector3 movementVector = Vector3.zero;
    private bool isFrozen = false;


    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        SetDirection(movementVector);
    }

    void FixedUpdate()
    {
        facingDirection.Normalize();//For inspector tweaks on play
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (!isFrozen)
        {
            playerBody.velocity = movementVector * speed;
        }
    }

    public void SetDirection(Vector3 direction)
    {
        if (!isFrozen)
        {
            movementVector = direction;

            if (direction != Vector3.zero)  //if it doesn't move we don't want him to change facing direction at all
            {
                facingDirection = direction;
            }
        }
    }

    public void SetFacingDirection(Vector3 direction)
    {
        if (!isFrozen)
        {
            facingDirection = direction;
            if (IsMoving())
            {
                movementVector = direction;
            }
        }
    }

    public Vector3 GetFacingDirection()//Called by CharacterView every frame
    {
        return facingDirection;
    }

    public Vector3 GetDirection()
    {
        return movementVector;
    }

    public bool IsMoving()
    {
        return movementVector != Vector3.zero;
    }

    public void Freeze()
    {
        movementVector = Vector3.zero;
        isFrozen = true;
    }

    public void Unfreeze()
    {
        isFrozen = false;
    }
}
