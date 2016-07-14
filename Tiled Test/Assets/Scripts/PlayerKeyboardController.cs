using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerKeyboardController : PlayerBaseController
{

	
	void FixedUpdate ()
    {
        ManageInputs();
    }


    void ManageInputs()
    {
        float horizontal = Input.GetAxis("Keyboard Horizontal");//we could cast this to an int to get only -1, 0 and 1
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
