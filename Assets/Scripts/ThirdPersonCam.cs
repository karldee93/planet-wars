using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow)) // when "right arrow" is pressed camera will rotate right around player
        {
            transform.Rotate(0, 0, 90 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) // when "left arrow" is pressed camera will rotate left around player
        {
            transform.Rotate(0, 0, -90 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow)) // when "left arrow" is pressed camera will rotate left around player
        {
            transform.Rotate(90 * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow)) // when "left arrow" is pressed camera will rotate left around player
        {
            transform.Rotate(-90 * Time.deltaTime, 0, 0);
        }
    }
}
