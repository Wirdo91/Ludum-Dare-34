using UnityEngine;
using System.Collections;

public class DrawGizmo : MonoBehaviour {

    void OnDrawGizmo()
    {
        Gizmos.DrawWireCube(this.transform.position, this.transform.localScale);
    }
}
