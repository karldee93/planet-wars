using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonOrbit : MonoBehaviour
{
    public GameObject planet1;
    public float orbitSpeed = 10;
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(planet1.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
    }
}
