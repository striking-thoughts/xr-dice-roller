using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAbove : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void LateUpdate()
    {
        this.transform.position = target.position + offset;
    }
}
