using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarController : MonoBehaviour
{
    public Slider healthSlider; // reference to slider component on the game object
    public GameObject HUD; // same but so can call scripts from canvas

    void Update()
    {
        
    }
    public void SetMaxHealth(float health) // gets health value from passed in value of maxHealth from movement script
    {
        healthSlider.maxValue = health; // Sets value of health bar slider to value of health
        healthSlider.value = health; // sets the actual visual health as the value of the passed in variable
    }

    public void SelfTerminate()
    {
        Destroy(gameObject);
    }
}
