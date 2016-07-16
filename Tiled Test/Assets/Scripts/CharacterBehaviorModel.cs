using UnityEngine;
using System.Collections;
using System;

public class CharacterBehaviorModel : MonoBehaviour {

    public float speed = 5f;

    private Rigidbody2D playerBody;
    private Vector3 movementVector = Vector3.down;
    private Vector3 facingDirection = Vector3.down;


    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        SetDirection(movementVector);
    }

    void Update () {
        UpdateDirection();
	}

    void FixedUpdate()
    {
        UpdateMovement();
    }



    void UpdateDirection()
    {
        
    }

    private void UpdateMovement()
    {
        playerBody.velocity = movementVector * speed;
    }

    public void SetDirection(Vector3 direction)
    {
        movementVector = direction;

        if (direction != Vector3.zero)  //if it doesn't move we don't want him to change facing direction at all
        {
            if (direction.x == 0 || direction.y == 0)   //We want to keep the facing direction if he goes in a diagonal angle
            {
                facingDirection = direction;
            }
        }
    }

    public Vector3 GetFacingDirection()
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

}
