using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBases : MonoBehaviour
{
    // set values to correspond with the amount of building given to each planet.
    public int planet1Base = 3;
    public int planet2Base = 4;
    public int planet3Base = 4;
    // allow the missile to be referenced.
    public GameObject missile;
   
    // checks to see if the base of a planet has been destoryed or not.
    public void CheckRemainingBuildings()
    {
        // if planet 1 has no building left to destroy run code.
        if (planet1Base == 0)
        {
            // access missile component missile script and set planet 1 destoryed to true.
            missile.GetComponent<MissileScript>().planet1Destoryed = true;
        }
        // these two work same as above but for the other two planets.
        else if(planet2Base == 0)
        {
            missile.GetComponent<MissileScript>().planet2Destroyed = true;
        }
        else if(planet3Base == 0)
        {
            missile.GetComponent<MissileScript>().planet3Destroyed = true;
        }
    }
}
