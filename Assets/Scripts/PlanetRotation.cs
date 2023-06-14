using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    public float rotationSpeed = 1f;
    void Update()
    {
        transform.Rotate(0, 0.2f * rotationSpeed, 0);
    }
}
