using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCamController : MonoBehaviour
{
    public Transform planet; // creates a variable to drop ball object into

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.RightArrow)) // when "right arrow" is pressed camera will rotate right around player
        {
            transform.RotateAround(planet.position, Vector3.up, 90 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) // when "left arrow" is pressed camera will rotate left around player
        {
            transform.RotateAround(planet.position, Vector3.up, -90 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow)) // when "left arrow" is pressed camera will rotate left around player
        {
            transform.RotateAround(planet.position, Vector3.right, 90 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow)) // when "left arrow" is pressed camera will rotate left around player
        {
            transform.RotateAround(planet.position, Vector3.right, -90 * Time.deltaTime);
        }
    }
}
