using UnityEngine;
using System.Collections;

public class InteractableBase : MonoBehaviour {

    public virtual void OnInteractFromLeftSide(Character character){ }
    public virtual void OnInteractFromRightSide(Character character) { }
    public virtual void OnInteractFromAbove(Character character) { }
    public virtual void OnInteractFromBelow(Character character) { }

}
