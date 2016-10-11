using UnityEngine;
using System.Collections;
using System;

public class StopBeacon : BeaconBase
{
    public FacingDirection facingDirection;

    public enum FacingDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public override void ArrivedToBeacon(CharacterBaseController characterController)
    {
        if (characterController is PatrollerAIController)
        {
            PatrollerAIController patrollerController = (PatrollerAIController)characterController;
            Vector3 direction = new Vector3();
            switch (facingDirection)
            {
                case FacingDirection.Up:
                    direction = Vector3.up;
                    break;
                case FacingDirection.Down:
                    direction = Vector3.down;
                    break;
                case FacingDirection.Left:
                    direction = Vector3.left;
                    break;
                case FacingDirection.Right:
                    direction = Vector3.right;
                    break;
            }
            patrollerController.WaitInPlace(direction);
        }
    }
}
