using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    // grav constant
    public float G = 800f;
    // this will stop it searching for more attractors each fixed update call
    public static List<Attractor> Attractors;

    public Rigidbody rb;

    void FixedUpdate()
    {
        // if the object this script is attached too has a tag of ground set gravity to 300.
        if (this.tag == "Ground")
        {
            G = 300f;
        }
        foreach (Attractor attractor in Attractors)
        {
            // if the attractor being looked at != to this attractor then call attract
            // this will stop it from trying to attract itself
            if (attractor != this)
                Attract(attractor);
        }
    }

    void OnEnable()
    {
        // initialise attractors array
        if (Attractors == null)
            Attractors = new List<Attractor>();
        // whenever an object is enabled add to list of attractors
        Attractors.Add(this);
    }

    void OnDisable()
    {
        Attractors.Remove(this);
    }

    void Attract(Attractor objToAttract)
    {
        // variable for the object to be attracted
        Rigidbody rbToAttract = objToAttract.rb;
        // gets the direction of the object to be attracted to the current object
        Vector3 direction = rb.position - rbToAttract.position;
        // this will get the distence between the 2 objects by getting the length of the direction vector using . mag
        float distance = direction.magnitude;
        // stop an error if obj is duplicated
        if (distance == 0f)
            return;
        // this will calc the mag of force
        // take mass of 1 obj * by the mass of the objtoatt / by distence between them squared
        // newtons cent equasion
        float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
        // apply force in direction of the obj with a strengh apply by newtons equasion
        Vector3 force = direction.normalized * forceMagnitude;
        // to apply the calced force
        rbToAttract.AddForce(force);
    }
}
