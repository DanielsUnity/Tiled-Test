using UnityEngine;
using System.Collections;
using System;

public class PlayerModel : MonoBehaviour {

    public float speed = 5f;

    private Rigidbody2D playerBody;
    private Vector3 movementVector = Vector3.down;


    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
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
    }

    public Vector3 GetDirection()
    {
        return movementVector;//TODO Flesh out GetDirection (Diagonals should keep last direction facing)
    }

    public bool IsMoving()
    {
        return movementVector != Vector3.zero;
    }

}
