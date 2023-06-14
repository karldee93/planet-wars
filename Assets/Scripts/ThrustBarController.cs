using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ThrustBarController : MonoBehaviour
{
    public Slider thrustSlider; // reference to slider component on the game object
    // this function is only called once from the missile script and is used to set the maximum allowed thrust.
    public void SetThrust(float thrust) // gets thrust value from passed in value of maxThrust from missile script
    {
        thrustSlider.maxValue = thrust; // Sets value of thrust bar slider to value of the thrust variable passed in
        thrustSlider.value = 0; // sets the actual visual thrust amount to 0 to begin with
    }
}
