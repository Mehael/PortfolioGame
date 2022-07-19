using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorDrawCube : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
