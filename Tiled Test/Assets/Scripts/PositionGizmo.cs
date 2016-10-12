using UnityEngine;
using System.Collections;

public class PositionGizmo : MonoBehaviour {

    [Range(0.1f, 3)]
    public float radius = 1;

    public Color color = Color.white;

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position,radius);
    }
}
