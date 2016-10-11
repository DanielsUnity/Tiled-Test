using UnityEngine;
using System.Collections;

public class PositionGizmo : MonoBehaviour {

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,1);
    }
}
