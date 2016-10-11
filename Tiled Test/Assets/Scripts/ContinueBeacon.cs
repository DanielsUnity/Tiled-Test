using UnityEngine;
using System.Collections;

public class ContinueBeacon : BeaconBase {

    public override void ArrivedToBeacon(CharacterBaseController characterController)
    {
        if(characterController is PatrollerAIController)
        {
            //don't do anything
        }
        
    }
}
