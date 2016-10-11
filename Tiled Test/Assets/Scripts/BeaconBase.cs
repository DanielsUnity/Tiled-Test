using UnityEngine;
using System.Collections;

public abstract class BeaconBase : MonoBehaviour {

    public abstract void ArrivedToBeacon(CharacterBaseController characterController); 
}
