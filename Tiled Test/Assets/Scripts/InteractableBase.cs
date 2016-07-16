using UnityEngine;
using System.Collections;

public class InteractableBase : MonoBehaviour {

    public virtual void OnInteractFromLeftSide(){}
    public virtual void OnInteractFromRightSide(){}
    public virtual void OnInteractFromAbove(){}
    public virtual void OnInteractFromBelow(){}

}
